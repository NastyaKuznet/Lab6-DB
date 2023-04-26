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
        public string Name { get; set; }//пришлось убрать private из-за jsonSerializer
        public Dictionary<string, PatternPropertyDB> Properties { get; set; }

        public PatternObjectDB(string nameFile)
        {
            ReadFile(nameFile);
        }

        public PatternObjectDB(string namePattern, Dictionary<string, PatternPropertyDB> prop) 
        {
            Name = namePattern;
            Properties = prop;
        }
        public PatternObjectDB() { }

        private void ReadFile(string nameFile)//надо ли это с нормальным способом считывания json?
        {

            //string[] data = File.ReadAllLines(nameFile);
            //for (int i = 0; i < data.Length; i++)
            //{
            //    string line = data[i];
            //    if (line.Contains("name") && Name == null)
            //    {
            //        Name = ReadQuotes(line);
            //    }
            //    if (line.Contains("columns"))
            //    {
            //        Properties = ReadProperties(data, i);
            //    }
            //}
        }
        private Dictionary<string, PatternPropertyDB> ReadProperties(string[] data, int indStart)
        {
            Dictionary<string, PatternPropertyDB> result = new Dictionary<string, PatternPropertyDB>();
            for (int i = indStart + 1; i < data.Length; i++)
            {
                string line = data[i];
                if (line.Contains('{'))
                {
                    PatternPropertyDB property = new PatternPropertyDB(data, i);
                    result[property.Name] = property;
                }
            }
            return result;
        }
        public static string ReadQuotes(string line)
        {
            int startSearch = line.IndexOf(':');
            int start = line.IndexOf('"', startSearch) + 1;
            int end = line.IndexOf('"', start);
            return line.Substring(start, end - start);
        }
    }
}
