using Lab6DB.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab6DB.ViewModel
{
    public class ItemTextBox : MyViewModel
    {
        public ObservableCollection<string> DeferredCollectionNameTables { get; set; }
        private Dictionary<string, ObservableCollection<string>> columnsTables;

        private string name;
        private ObservableCollection<string> elementsComboBoxTypeColumn = new ObservableCollection<string>() { "int", "string", "dateTime" };
        private string selectedElementComboBoxType = "";
        private bool isForeignKey;
        private ObservableCollection<string> collectionNameTables;
        private string selectedNameTable;
        private ObservableCollection<string> collectionNameColumns;
        private string selectedNameColumn;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        public ObservableCollection<string> ElementsComboBoxTypeColumn
        {
            get { return elementsComboBoxTypeColumn; }
            set
            {
                elementsComboBoxTypeColumn = value;
                OnPropertyChanged(nameof(elementsComboBoxTypeColumn));
            }
        }
        public string SelectedElementComboBoxType
        {
            get { return selectedElementComboBoxType; }
            set { selectedElementComboBoxType = value; OnPropertyChanged(nameof(SelectedElementComboBoxType)); }
        }
        public bool IsForeignKey
        {
            get { return isForeignKey; }
            set 
            { 
                isForeignKey = value; 
                OnPropertyChanged(nameof(IsForeignKey)); 
                CollectionNameTables = DeferredCollectionNameTables; 
            }
        }
        public ObservableCollection<string> CollectionNameTables
        {
            get { return collectionNameTables; }
            set { collectionNameTables = value; OnPropertyChanged(nameof(CollectionNameTables)); }
        }
        public string SelectedNameTable
        {
            get { return selectedNameTable; }
            set { selectedNameTable = value; OnPropertyChanged(nameof(SelectedNameTable)); }
        }
        public ObservableCollection<string> CollectionNameColumns
        {
            get { return collectionNameColumns; }
            set { collectionNameColumns = value; OnPropertyChanged(nameof(CollectionNameColumns)); }
        }
        public string SelectedNameColumns
        {
            get { return selectedNameColumn; }
            set { selectedNameColumn = value; OnPropertyChanged(nameof(SelectedNameColumns)); }
        }

        public ItemTextBox(string _name, ObservableCollection<string> collection, Dictionary<string, ObservableCollection<string>> _columnsTables)
        {
            Name = _name;
            DeferredCollectionNameTables = collection;
            columnsTables = _columnsTables;
        }
        public ItemTextBox(string _name, string type, bool isForeignKey)
        {
            Name = _name;
            SelectedElementComboBoxType = type;
            IsForeignKey = isForeignKey;
        }

        public ICommand SelectTable
        {
            get 
            {
                return new CommandDelegate(parametr =>
                {
                    foreach(string nameTable in columnsTables.Keys)
                    {
                        if(nameTable.CompareTo(SelectedNameTable) == 0)
                            CollectionNameColumns = columnsTables[nameTable];
                    }
                });
            }
        }
    }
}
