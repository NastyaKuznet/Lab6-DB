using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6DB.Model
{
    public class PatternPropertyDB
    {
        public string Name { get; set; }//пришлось убрать private из-за jsonSerializer
        public string _Type { get; set; }
        public PropertyType Type { get; set; }

        public PatternPropertyDB(string[] data, int indStart)
        {
            ReadProperty(data, indStart);
        }
        public PatternPropertyDB(string nameProp, PropertyType typeProp) 
        {
            Name= nameProp;
            Type = typeProp;
        }
        public PatternPropertyDB() { }

        private void ReadProperty(string[] data, int indStart)
        {
            for (int i = indStart; i < data.Length; i++)
            {
                string line = data[i];
                if (line.Contains('}'))
                    break;
                if (line.Contains("name"))
                    Name = PatternObjectDB.ReadQuotes(line);
                if (line.Contains("type"))
                    ChoiceType(PatternObjectDB.ReadQuotes(line));
            }
        }

        private void ChoiceType(string type)
        {
            switch (type)
            {
                case "int":
                    Type = PropertyType.Int;
                    break;
                case "string":
                    Type = PropertyType.String;
                    break;
                case "datatime":
                    Type = PropertyType.DataTime;
                    break;
            }
        }

        public enum PropertyType
        {
            Int, String, DataTime
        }
    }
}
