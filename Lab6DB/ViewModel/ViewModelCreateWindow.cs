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
using Lab6DB.Model.PrimaryData;

namespace Lab6DB.ViewModel
{
    public class ViewModelCreateWindow: MyViewModel
    {
        public CreateWindow CreateWindow { get; set; }

        private string fullFolderPath = "";
        private string nameTable;
        private string numberColumns;
        private ObservableCollection<ItemTextBox> columns = new ObservableCollection<ItemTextBox>();
        private ObservableCollection<string> elementsComboBoxTypeColumn = new ObservableCollection<string>() {"Int","String","DateTime"};
        private string state = "";

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
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public ICommand CreateTextBoxes
        {
            get
            {
                return new CommandDelegate(parametr =>
                {
                    State = CheckError.ErrorEmptyString(NumberColumns);
                    if (State.Contains(CheckError.NotError))
                    {
                        State = CheckError.InputErrorInt(NumberColumns);
                        if (State.Contains(CheckError.NotError))
                        {
                            int numberColumn = int.Parse(NumberColumns);
                            for (int i = 0; i < numberColumn; i++)
                            {
                                Columns.Add(new ItemTextBox(""));
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
                    State = CheckError.ErrorEmptyString(NameTable);
                    if (State.Contains(CheckError.NotError))
                    {
                        DataTable newTable = new DataTable(NameTable);
                        Dictionary<string, PatternPropertyDB> props = new Dictionary<string, PatternPropertyDB>();
                        foreach (ItemTextBox contentColumn in Columns)
                        {
                            State = CheckError.ErrorEmptyString(contentColumn.Name);
                            if (State.Contains(CheckError.NotError))
                            {
                                DataColumn column = new DataColumn(contentColumn.Name);
                                props[contentColumn.Name] = new PatternPropertyDB(contentColumn.Name, contentColumn.SelectedElementComboBoxType);
                                newTable.Columns.Add(column);
                            }
                            else { break; }
                        }
                        if (State.Contains(CheckError.NotError))
                        {
                            NewPattern = new PatternObjectDB(NameTable, props);//а надо ли в виде свойства?
                            string json = JsonSerializer.Serialize(NewPattern);
                            File.WriteAllText($"{FullFolderPath}\\{NameTable}.json", json);
                            //State = $"Таблица <{NameTable}> сохранена!";
                        }
                    }
                    CreateWindow.Close();
                });
            }
        }

    }
}
