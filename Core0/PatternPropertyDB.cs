using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0
{
    public class PatternPropertyDB
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsPrimaryKey { get; set; }
        public List<string> ReferencingTables { get; set; }
        public bool IsForeignKey { get; set; }
        public string ForeignTable { get; set; }
        public string ForeignColumn { get; set; }

        public PatternPropertyDB(string nameProp, string typeProp,bool isPrimaryKey, bool isForeignKey, string foreignTable, string foreignColumn)
        {
            Name = nameProp;
            Type = typeProp;
            IsPrimaryKey = isPrimaryKey;
            IsForeignKey = isForeignKey;
            ForeignTable = foreignTable;
            ForeignColumn = foreignColumn;
        }
        public PatternPropertyDB() { }


        public enum PropertyType
        {
            Int, String, DataTime
        }
    }
}
