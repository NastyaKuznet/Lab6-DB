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

namespace Lab6DB.ViewModel
{
    public class ViewModelCreateWindow: MyViewModel
    {

        private string fullFolderPath = "";
        private string nameTable;
        private string numberColumns;
        private ObservableCollection<ItemTextBox> columns = new ObservableCollection<ItemTextBox>();
        private ObservableCollection<string> elementsComboBoxTypeColumn = new ObservableCollection<string>() {"Int","String","Date"};


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

        public ICommand CreateTextBoxes
        {
            get
            {
                return new CommandDelegate(parametr =>
                {
                    //проверка что колличество столбов введенно int
                    int numberColumn = int.Parse(NumberColumns);
                    for(int i = 0; i < numberColumn; i++)
                    {
                        Columns.Add(new ItemTextBox(""));
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
                    DataTable newTable = new DataTable(NameTable);
                    Dictionary<string, PatternPropertyDB> props = new Dictionary<string, PatternPropertyDB>();

                    foreach (ItemTextBox contentColumn in Columns)
                    {
                        DataColumn column = new DataColumn(contentColumn.Name);
                        props[contentColumn.Name] = new PatternPropertyDB(contentColumn.Name, SelectType(contentColumn.SelectedElementComboBoxType));
                        newTable.Columns.Add(column);
                    }
                    PatternObjectDB newPattern = new PatternObjectDB(NameTable, props);
                });
            }
        }

        private PatternPropertyDB.PropertyType SelectType(string type)
        {
            switch(type)
            {
                case ("Int"):
                    return PatternPropertyDB.PropertyType.Int;
                case ("String"):
                    return PatternPropertyDB.PropertyType.String;
                default:
                    return PatternPropertyDB.PropertyType.DataTime;
            }
        }

    }
}
