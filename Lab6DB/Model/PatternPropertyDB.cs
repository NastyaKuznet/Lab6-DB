using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6DB.Model
{
    public class PatternPropertyDB
    {
        public string Name { get; set; }//пришлось убрать private set из-за jsonSerializer
        public string Type { get; set; }

        public PatternPropertyDB(string nameProp, string typeProp) 
        {
            Name= nameProp;
            Type = typeProp;
        }
        public PatternPropertyDB() { }


        public enum PropertyType
        {
            Int, String, DataTime
        }
    }
}
