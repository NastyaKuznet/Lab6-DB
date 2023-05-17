using Lab6DB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.Json;
using System.IO;
using Core0;

namespace Lab6DB.ViewModel
{
    public class ViewModelCreateWindow: MyViewModel
    {
        public CreateWindow CreateWindow { get; set; }
        public ObservableCollection<string> CollectionNameTables { get; set; }
        public ObservableCollection<ElementDB> ElementDBs { get; set; }

        private string fullFolderPath = "";
        private string nameTable;
        private string numberColumns;
        private ObservableCollection<ItemTextBox> columns = new ObservableCollection<ItemTextBox>();
        private ObservableCollection<string> elementsComboBoxTypeColumn = new ObservableCollection<string>() {"Int","String","DateTime"};
        private string contentErrorWindow = CheckError.NotError;
        

        public PatternObjectDB NewPattern = new PatternObjectDB();

        public string FullFolderPath
        {
            get { return fullFolderPath; }
            set
            {
                fullFolderPath = value;
                OnPropertyChanged(nameof(FullFolderPath));
            }
        }
        public string NameTable
        {
            get { return nameTable; }
            set
            {
                nameTable = value;
                OnPropertyChanged(nameof(NameTable));
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

        public ObservableCollection<ItemTextBox> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }
        public ObservableCollection<string> ElementsComboBoxTypeColumn
        {
            get { return elementsComboBoxTypeColumn; }
            set
            {
                elementsComboBoxTypeColumn = value;
                OnPropertyChanged(nameof(ElementsComboBoxTypeColumn));
            }
        }
        public string ContentErrorWindow
        {
            get { return contentErrorWindow; }
            set
            {
                contentErrorWindow = value;
                OnPropertyChanged(nameof(ContentErrorWindow));
            }
        }


        public ICommand CreateTextBoxes
        {
            get
            {
                return new CommandDelegate(parametr =>
                {
                    ContentErrorWindow = CheckError.ErrorEmptyString(NumberColumns);
                    if (ContentErrorWindow.Contains(CheckError.NotError))
                    {
                        ContentErrorWindow = CheckError.InputErrorInt(NumberColumns);
                        if (ContentErrorWindow.Contains(CheckError.NotError))
                        {
                            int numberColumn = int.Parse(NumberColumns);
                            for (int i = 0; i < numberColumn; i++)
                            {
                                Columns.Add(new ItemTextBox("", CollectionNameTables, CreateCollectColumnsTables()));
                            }
                        }
                    }
                });
            }
            
        }
        public ICommand Save
        {
            get 
            {
                return new CommandDelegate(parametr => 
                {
                    ContentErrorWindow = CheckError.ErrorEmptyString(NameTable);
                    if (ContentErrorWindow.Contains(CheckError.NotError))
                    {
                        DataTable newTable = new DataTable(NameTable);
                        Dictionary<string, PatternPropertyDB> props = new Dictionary<string, PatternPropertyDB>();
                        int id = 1;
                        foreach (ItemTextBox contentColumn in Columns)
                        {
                            ContentErrorWindow = CheckError.ErrorEmptyString(contentColumn.Name);
                            ContentErrorWindow = CheckError.ErrorTypeColumnIsForeignKey(contentColumn.Name, contentColumn.IsForeignKey, contentColumn.SelectedElementComboBoxType);
                            if (ContentErrorWindow.CompareTo(CheckError.NotError) == 0)
                            {
                                DataColumn column = new DataColumn(contentColumn.Name);
                                props[contentColumn.Name] = new PatternPropertyDB(id, contentColumn.Name, contentColumn.SelectedElementComboBoxType, contentColumn.IsForeignKey);
                                newTable.Columns.Add(column);
                            }
                            else { break; }
                            id++;
                        }
                        if (ContentErrorWindow.Contains(CheckError.NotError))
                        {
                            NewPattern = new PatternObjectDB(NameTable, props);
                            string json = JsonSerializer.Serialize(NewPattern);
                            File.WriteAllText($"{FullFolderPath}\\{NameTable}.json", json);
                            CreateWindow.Close();
                        }
                    } 
                });
            }
        }

        private Dictionary<string, ObservableCollection<string>> CreateCollectColumnsTables()
        {
            Dictionary<string, ObservableCollection<string>> dictionary = new Dictionary<string, ObservableCollection<string>>(); 
            foreach(ElementDB element in ElementDBs)
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

    }
}
