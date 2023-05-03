using Lab6DB.Model;
using Lab6DB.Model.AdditionalData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab6DB.ViewModel
{
    public class ViewModelRewriteDataWindow: MyViewModel
    {
        private DataTable selectedTable;
        private string сontentErrorWindow = CheckError.NotError;
        private ObservableCollection<string> collectDelete;
        private string selectedDelete;

        public DataTable SelectedTable
        {
            get 
            { 
                return selectedTable; 
            }
            set 
            { 
                selectedTable = value; 
                OnPropertyChanged(nameof(SelectedTable)); 
            }
        }
        public string ContentErrorWindow
        {
            get { return сontentErrorWindow; }
            set
            {
                сontentErrorWindow = value;
                OnPropertyChanged(nameof(ContentErrorWindow));
            }
        }
        public ObservableCollection<string> CollectDelete
        {
            get { return collectDelete; }
            set
            {
                collectDelete = value;
                OnPropertyChanged(nameof(CollectDelete));
            }
        }
        public string SelectedDelete
        {
            get { return selectedDelete; }
            set
            {
                selectedDelete = value;
                OnPropertyChanged(nameof(SelectedDelete));
            }
        }

        public string NameTable { get; set; }
        public ElementDB Element { get; set; }


        public ICommand Save
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    Element.Table = SelectedTable;
                    CollectDelete = RewriteCollectionDelete(Element);
                    CreateCSV();
                });
            }
        }
        private void CreateCSV()
        {
            string[] lines = new string[SelectedTable.Rows.Count];
            for(int i = 0; i < lines.Length; i++)
            {
                DataRow row = SelectedTable.Rows[i];
                int countEl = Element.Pattern.Properties.Count;
                string[] elements = new string[countEl];
                
                for (int j = 0; j < countEl; j++)
                {
                    elements[j] = row[j].ToString();
                    ContentErrorWindow = CheckError.ErrorEmptyString(elements[j]);
                    if (ContentErrorWindow.CompareTo(CheckError.NotError) != 0)
                        break;
                }
                if (ContentErrorWindow.CompareTo(CheckError.NotError) != 0)
                    break;
                ContentErrorWindow = CheckError.InputErrorProperties(elements, Element.Pattern);
                if (ContentErrorWindow.CompareTo(CheckError.NotError) != 0)
                    break;
                string line = string.Join(';', elements);
                lines[i] = line;
                
            }
            if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
            {
                if (Element.WayCSV == null)
                {
                    string newWayCSV = Element.WayJson.Substring(0, Element.WayJson.LastIndexOf('\\')) + "\\" + Element.Table.TableName + ".csv";
                    File.WriteAllLines(newWayCSV, lines);
                }
                else
                    File.WriteAllLines(Element.WayCSV, lines);
            }
        }

        private ObservableCollection<string> RewriteCollectionDelete(ElementDB element)
        {
            ObservableCollection<string> collect = new ObservableCollection<string>();
            foreach (DataRow row in element.Table.Rows)
            {
                collect.Add(row[0].ToString());
            }
            return collect;
        }

        public ICommand Delete
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    DataTable rewriteTable = new DataTable(SelectedTable.TableName);
                    rewriteTable = RewriteColumns(rewriteTable);
                    foreach(DataRow row in SelectedTable.Rows)
                    {
                        if (row[0].ToString().CompareTo(SelectedDelete) == 0)
                            continue;
                        rewriteTable = RewriteRow(row, rewriteTable);
                    }
                    SelectedTable = rewriteTable;
                    Element.Table = rewriteTable;
                    CreateCSV();
                });
            }
        }

        private DataTable RewriteColumns(DataTable table)
        {
            foreach(DataColumn column in SelectedTable.Columns)
            {
                table.Columns.Add(column.ColumnName);
            }
            return table;
        }
        private DataTable RewriteRow(DataRow row, DataTable table)
        {
            DataRow newRow = table.NewRow();
            foreach(DataColumn column in table.Columns)
            {
                newRow[column.ColumnName] = row[column.ColumnName];
            }
            table.Rows.Add(newRow);
            return table;
        }
    }
}
