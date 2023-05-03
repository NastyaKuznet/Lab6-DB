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
using Lab6DB.Model.PrimaryData;
using Lab6DB.Model.AdditionalData;

namespace Lab6DB.Model
{
    public class LoaderFiles
    {
        public string State = CheckError.NotError;

        private Dictionary<string, AdditionalDataPattern> patterns = new Dictionary<string, AdditionalDataPattern>();

        public Dictionary<string, AdditionalDataPattern> Patterns { get { return patterns; } }

        private Dictionary<string, AdditionalDataObject> objects = new Dictionary<string, AdditionalDataObject>();
        public Dictionary<string, AdditionalDataObject> Objects { get { return objects; } }


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
                if (State.CompareTo(CheckError.NotError) == 0)
                    ConnectTemplateObjectAndCreate(nameFilesObjects);
            }
        }

        private void СreatePatterns(List<string> nameFilesPatterns)
        {
            State = CheckError.IsNotHavePatterns(nameFilesPatterns);
            if (State.CompareTo(CheckError.NotError) == 0)
            {
                Dictionary<string, AdditionalDataPattern> _patterns = new Dictionary<string, AdditionalDataPattern>();

                foreach (string name in nameFilesPatterns)
                {
                    string contentJsonFile = File.ReadAllText(name);
                    PatternObjectDB pattern = JsonSerializer.Deserialize<PatternObjectDB>(contentJsonFile);
                    _patterns[pattern.Name] = new AdditionalDataPattern(name, pattern);
                }
                patterns = _patterns;
            }
        }

        private void ConnectTemplateObjectAndCreate(List<string> nameFilesObjects)
        {
            State = CheckError.IsNotHaveObjects(nameFilesObjects);
            if (State.CompareTo(CheckError.NotError) == 0)
            {
                Dictionary<string, AdditionalDataObject> _objects = new Dictionary<string, AdditionalDataObject>();

                foreach (string name in nameFilesObjects)
                {
                    int lastSymbol = name.LastIndexOf('\\') + 1;
                    string correctName = name.Substring(lastSymbol, name.LastIndexOf('.') - lastSymbol);
                    if (patterns.ContainsKey(correctName))
                    {
                        AdditionalDataObject addObjectDB = LoaderData.ReadObjectDB(name, patterns[correctName].Pattern);

                        State = LoaderData.State;
                        if (State.CompareTo(CheckError.NotError) != 0)
                        {
                            break;
                        }
                        _objects[correctName] = addObjectDB;
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
