using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6DB.ViewModel
{
    public class ItemTextBox: MyViewModel
    {
        
        private string name;
        private ObservableCollection<string> elementsComboBoxTypeColumn = new ObservableCollection<string>() { "Int", "String", "Data" };
        private string selectedElementComboBoxType = "";

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
        public ItemTextBox(string _name) 
        {
            Name = _name;
        }

    }
}
