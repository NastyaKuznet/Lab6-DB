using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab6DB.Model;

namespace Lab6DB.Model
{
    public static class LoaderData
    {
        public static string State { get; private set; }
        public static ObjectDB[] ReadObjectDB(string nameFile, PatternObjectDB patternObjectDB)
        {
            string[] data = File.ReadAllLines(nameFile);
            ObjectDB[] objectDBs = new ObjectDB[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                string[] line = data[i].Split(';');
                State = CheckError.InputErrorRightCountColumns(line, patternObjectDB.Properties.Count, nameFile);
                if (State.Contains(CheckError.NotError))
                {
                    State = CheckError.InputErrorProperties(line, patternObjectDB);
                    if (State.Contains(CheckError.NotError))
                        objectDBs[i] = new ObjectDB(patternObjectDB, line);
                    else
                        return objectDBs;

                }
                else
                    return objectDBs;
            }
            return objectDBs;
        }
    }
}
