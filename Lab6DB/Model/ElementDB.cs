using Lab6DB.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core0;


namespace Lab6DB.Model
{
    public class ElementDB
    {
        public string WayJson { get; set; }
        public string WayCSV { get; set; }
        public PatternObjectDB Pattern { get; set; }
        public ObservableCollection<ItemTextBox> Columns { get; set; }
        public DataTable Table { get; set; }
        public ElementDB(string wayJson, string wayCSV, PatternObjectDB pattern, ObservableCollection<ItemTextBox> columns, DataTable table)
        {
            WayJson = wayJson;
            WayCSV = wayCSV;
            Columns = columns;
            Pattern = pattern;
            Table = table;
        }
        public ElementDB() { }
    }
}
