using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab6DB.Model;

namespace Lab6DB.Model
{
    public class PatternObjectDB
    {
        public string Name { get; set; }//пришлось убрать private set из-за jsonSerializer
        public Dictionary<string, PatternPropertyDB> Properties { get; set; }

        public PatternObjectDB(string namePattern, Dictionary<string, PatternPropertyDB> prop) 
        {
            Name = namePattern;
            Properties = prop;
        }
        public PatternObjectDB() { }

    }
}
