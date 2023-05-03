using Lab6DB.Model.AdditionalData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.Json;
using System.IO;
using System.Collections.ObjectModel;
using Lab6DB.Model.PrimaryData;
using Lab6DB.Model;
using static System.Windows.Forms.AxHost;

namespace Lab6DB.ViewModel
{
    public class ViewModelRewriteTableWindow: MyViewModel
    {   
        private string nameTable;
        private ObservableCollection<ItemTextBox> columns = new ObservableCollection<ItemTextBox>();
        private string numberColumns;
        private ObservableCollection<string> nameColumns;
        private string selectedColumn;
        private string сontentErrorWindow;


        public ElementDB Element { get; set; }
        public ObservableCollection<ItemTextBox> OldColumns { get; set;}

        public string NameTable
        {
            get { return nameTable; }
            set
            {
                nameTable = value;
                OnPropertyChanged(nameof(NameTable));
            }
        }
        public ObservableCollection<ItemTextBox> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }
        public string NumberColumns
        {
            get { return numberColumns; }
            set
            {
                numberColumns = value;
                OnPropertyChanged(nameof(NumberColumns));
            }
        }
        public ObservableCollection<string> NameColumns
        {
            get { return nameColumns; }
            set
            {
                nameColumns = value;
                OnPropertyChanged(nameof(NameColumns));
            }
        }
        public string SelectedColumn
        {
            get { return selectedColumn; }
            set { selectedColumn = value; OnPropertyChanged(nameof(SelectedColumn)); }
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



        public ICommand RewriteNameTable
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    Element.Pattern.Name = NameTable;
                    string upJson = JsonSerializer.Serialize(Element.Pattern);
                    File.WriteAllText(Element.WayJson, upJson);
                });
            }
        }

        public ICommand AddColumn
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    ContentErrorWindow = CheckError.ErrorEmptyString(NumberColumns);
                    if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
                    {
                        ContentErrorWindow = CheckError.InputErrorInt(NumberColumns);
                        if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
                        {
                            int numberColumn = int.Parse(NumberColumns);
                            for (int i = 0; i < numberColumn; i++)
                            {
                                Columns.Add(new ItemTextBox(""));
                                NameColumns.Add("");
                            }
                        }
                    }
                });
            }
        }
        private Dictionary<string, PatternPropertyDB> newProp = new Dictionary<string, PatternPropertyDB>();
        public ICommand RewriteColumnTable
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    DataTable rewriteTable = new DataTable(Element.Table.TableName);
                    
                    if (Columns.Count < OldColumns.Count)
                    {
                        rewriteTable = RewriteData(rewriteTable, Columns);
                    }
                    else
                    {
                        rewriteTable = RewriteData(rewriteTable, OldColumns);
                        if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
                        {
                            if (Columns.Count > OldColumns.Count)
                            {
                                for (int i = newProp.Count; i < Columns.Count; i++)
                                {
                                    newProp[Columns[i].Name] = new PatternPropertyDB(Columns[i].Name, Columns[i].SelectedElementComboBoxType);
                                    rewriteTable.Columns.Add(new DataColumn(Columns[i].Name));
                                    NameColumns[i] = Columns[i].Name;
                                }
                            }
                        }
                    }
                    if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
                    {
                        Element.Table = rewriteTable;
                        Element.Pattern.Properties = newProp;
                        
                        string upJson = JsonSerializer.Serialize(Element.Pattern);
                        File.WriteAllText(Element.WayJson, upJson);
                    }
                });
            }
        }
        private DataTable RewriteData(DataTable rewriteTable, ObservableCollection<ItemTextBox> collection)
        {
            ContentErrorWindow = CheckUniqueNameColumnsAndNotEmptyType(Columns);
            if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    ItemTextBox itemColumn = Columns[i];
                    newProp[collection[i].Name] = new PatternPropertyDB(itemColumn.Name, itemColumn.SelectedElementComboBoxType);
                    NameColumns[i] = itemColumn.Name;

                    Element.Columns[i] = itemColumn;
                    DataColumn rewriteColumn = new DataColumn(itemColumn.Name);
                    rewriteTable.Columns.Add(rewriteColumn);
                }
            }
            return rewriteTable;
        }
        private string CheckUniqueNameColumnsAndNotEmptyType(ObservableCollection<ItemTextBox> collection)
        {
            for(int i = 0; i < collection.Count; i++)
            {
                for(int j = i + 1; j < collection.Count; j++)
                {
                    if (collection[i].Name.CompareTo(collection[j].Name) == 0)
                        return "Названия столбцов должны быть уникальными.";
                }
                if (string.IsNullOrEmpty(collection[i].Name))
                    return "Заполните названия всех столбцов.";
                if (string.IsNullOrEmpty(collection[i].SelectedElementComboBoxType))
                    return $"Выберете тип данных столбца {collection[i].Name}.";
                
            }
            return CheckError.NotError;
        }

        public ICommand DeleteColumn
        {
            get
            {
                return new CommandDelegate(parameter =>
                {
                    for(int i = 0; i < Columns.Count; i++)
                    {
                        if (!Element.Table.Columns.Contains(Columns[i].Name))
                        {
                            ContentErrorWindow = "Сперва сохраните добавленные столбцы.";
                            break;
                        }
                        if (Columns[i].Name.CompareTo(SelectedColumn) == 0)
                        {
                            
                            NameColumns.RemoveAt(i);
                            Element.Pattern.Properties.Remove(Columns[i].Name);
                            Columns.RemoveAt(i);
                            Element.Table.Columns.RemoveAt(i);
                        }
                    }
                });
            }
        }
    }
}
