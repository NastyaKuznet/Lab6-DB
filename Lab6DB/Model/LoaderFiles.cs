using Lab6DB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Text.Json;

namespace Lab6DB.Model
{
    public class LoaderFiles
    {
        public string State;

        private Dictionary<string, PatternObjectDB> patterns = new Dictionary<string, PatternObjectDB>();

        public Dictionary<string, PatternObjectDB> Patterns { get { return patterns; } }

        private Dictionary<string, ObjectDB[]> objects = new Dictionary<string, ObjectDB[]>();
        public Dictionary<string, ObjectDB[]> Objects { get { return objects; } }


        public LoaderFiles(string nameFolder)
        {
            SeparatePatternsObjects(nameFolder);
        }

        private void SeparatePatternsObjects(string nameFolder)
        {
            if (!string.IsNullOrEmpty(nameFolder))
            {
                List<string> nameFilesPatterns = new List<string>();
                List<string> nameFilesObjects = new List<string>();

                foreach (var item in new DirectoryInfo(nameFolder).GetFiles())
                {
                    if (item.Name.Contains(".json"))
                        nameFilesPatterns.Add(item.FullName);
                    if (item.Name.Contains(".csv"))
                        nameFilesObjects.Add(item.FullName);
                }

                СreatePatterns(nameFilesPatterns);
                if (State.Contains(CheckError.NotError))
                    ConnectTemplateObjectAndCreate(nameFilesObjects);
            }
        }

        private void СreatePatterns(List<string> nameFilesPatterns)
        {
            State = CheckError.IsNotHavePatterns(nameFilesPatterns);
            if (State.Contains(CheckError.NotError))
            {
                Dictionary<string, PatternObjectDB> _patterns = new Dictionary<string, PatternObjectDB>();

                foreach (string name in nameFilesPatterns)
                {
                    string contentJsonFile = File.ReadAllText(name);
                    PatternObjectDB pattern = JsonSerializer.Deserialize<PatternObjectDB>(contentJsonFile);
                    //PatternObjectDB pattern = new PatternObjectDB(name);
                    _patterns[pattern.Name] = pattern;
                }
                patterns = _patterns;
            }
        }

        private void ConnectTemplateObjectAndCreate(List<string> nameFilesObjects)
        {
            State = CheckError.IsNotHaveObjects(nameFilesObjects);
            if (State.Contains(CheckError.NotError))
            {
                Dictionary<string, ObjectDB[]> _objects = new Dictionary<string, ObjectDB[]>();

                foreach (string name in nameFilesObjects)
                {
                    int lastSymbol = name.LastIndexOf('\\') + 1;
                    string correctName = name.Substring(lastSymbol, name.LastIndexOf('.') - lastSymbol);
                    if (patterns.ContainsKey(correctName))
                    {
                        ObjectDB[] objectD = LoaderData.ReadObjectDB(name, patterns[correctName]);
                        State = LoaderData.State;
                        if (!State.Contains(CheckError.NotError))
                        {
                            break;
                        }
                        _objects[correctName] = objectD;
                    }
                    else
                    {
                        State = CheckError.MismatchPattensAndObjects;
                    }
                }
                objects = _objects;
            }
        }
    }
}
