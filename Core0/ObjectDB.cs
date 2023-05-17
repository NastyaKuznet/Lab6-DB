using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0
{
    public class ObjectDB
    {
        public string Name { get; private set; }
        public Dictionary<string, string> Property { get; private set; }
        public ObjectDB(PatternObjectDB patternObjectDB, string[] lineData)
        {
            Name = patternObjectDB.Name;
            int i = 0;
            Dictionary<string, string> propertyDB = new Dictionary<string, string>();
            foreach (PatternPropertyDB prop in patternObjectDB.Properties.Values)
            {
                propertyDB[prop.Name] = lineData[i];
                i++;
            }
            Property = propertyDB;
        }
    }
}
