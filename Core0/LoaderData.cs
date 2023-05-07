using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0
{
    public static class LoaderData
    {
        public static string State = CheckError.NotError;
        public static AdditionalDataObject ReadObjectDB(string nameFile, PatternObjectDB patternObjectDB)
        {
            AdditionalDataObject additionalDataObject = new AdditionalDataObject();
            string[] data = File.ReadAllLines(nameFile);
            ObjectDB[] objectDBs = new ObjectDB[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                string[] line = data[i].Split(';');
                State = CheckError.InputErrorRightCountColumns(line, patternObjectDB.Properties.Count, nameFile);
                if (State.CompareTo(CheckError.NotError) == 0)
                {
                    State = CheckError.InputErrorProperties(line, patternObjectDB);
                    if (State.CompareTo(CheckError.NotError) == 0)
                        objectDBs[i] = new ObjectDB(patternObjectDB, line);
                    else
                        return additionalDataObject;

                }
                else
                    return additionalDataObject;
            }
            additionalDataObject.WayCSV = nameFile;
            additionalDataObject.DBObject = objectDBs;
            return additionalDataObject;
        }
    }
}
