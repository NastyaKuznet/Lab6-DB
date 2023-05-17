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
using Core0;
using Lab6DB.Model;
using static System.Windows.Forms.AxHost;

namespace Lab6DB.ViewModel
{
    public class ViewModelRewriteTableWindow: MyViewModel
    {
        private string nameTable;
        private ObservableCollection<ItemTextBox> columns = new ObservableCollection<ItemTextBox>();
        private string numberColumns = "";
        private ObservableCollection<string> nameColumns = new ObservableCollection<string>();
        private string selectedColumn = "";
        private string сontentErrorWindow = CheckError.NotError;

        public ElementDB Element { get; set; }
        public ObservableCollection<ElementDB> ElementDBs { get; set; }
        public ObservableCollection<ItemTextBox> OldColumns { get; set;}
        public ObservableCollection<string> CollectionNameTables { get; set; }

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
                                Columns.Add(new ItemTextBox("", CollectionNameTables, CreateCollectColumnsTables()));
                                NameColumns.Add("");
                            }
                        }
                    }
                });
            }
        }
        private Dictionary<string, ObservableCollection<string>> CreateCollectColumnsTables()
        {
            Dictionary<string, ObservableCollection<string>> dictionary = new Dictionary<string, ObservableCollection<string>>();
            foreach (ElementDB element in ElementDBs)
            {
                ObservableCollection<string> collect = new ObservableCollection<string>();
                foreach (string column in element.Pattern.Properties.Keys)
                {
                    collect.Add(column);
                }
                dictionary[element.Pattern.Name] = collect;
            }
            return dictionary;
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
                                    ContentErrorWindow = CheckError.ErrorTypeColumnIsForeignKey(Columns[i].Name,Columns[i].IsForeignKey, Columns[i].SelectedElementComboBoxType);
                                    if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
                                    {
                                        newProp[Columns[i].Name] = new PatternPropertyDB(i, Columns[i].Name, Columns[i].SelectedElementComboBoxType, Columns[i].IsForeignKey);
                                        rewriteTable.Columns.Add(new DataColumn(Columns[i].Name));
                                        NameColumns[i] = Columns[i].Name;
                                    }
                                    else break;
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
                        ContentErrorWindow = "Сохранено.";
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
                    ContentErrorWindow = CheckError.ErrorTypeColumnIsForeignKey(itemColumn.Name, itemColumn.IsForeignKey, itemColumn.SelectedElementComboBoxType);
                    if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
                    {
                        newProp[collection[i].Name] = new PatternPropertyDB(i + 1, itemColumn.Name, itemColumn.SelectedElementComboBoxType, itemColumn.IsForeignKey);
                        NameColumns[i] = itemColumn.Name;

                        Element.Columns[i] = itemColumn;
                        DataColumn rewriteColumn = new DataColumn(itemColumn.Name);
                        rewriteTable.Columns.Add(rewriteColumn);
                    }
                    else break;
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
