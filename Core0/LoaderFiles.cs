using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json;


namespace Core0
{
    public class LoaderFiles
    {
        public string State = CheckError.NotError;
        public ObservableCollection<ElementDB> Elements = new ObservableCollection<ElementDB>();

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

                СreateElementDB(nameFilesPatterns, nameFilesObjects);
            }
        }
        private void СreateElementDB(List<string> nameFilesPatterns, List<string> nameFilesObjects)
        {
            if (IsHaveError(CheckError.ErrorNotHavePatterns(nameFilesPatterns)))
            {
                foreach (string name in nameFilesPatterns)
                {
                    ElementDB element = new ElementDB();

                    string contentJsonFile = File.ReadAllText(name);
                    PatternObjectDB pattern = JsonSerializer.Deserialize<PatternObjectDB>(contentJsonFile);

                    element.FullPattern = new AdditionalDataPattern(name, pattern);
                    element = ConnectTemplateObjectAndCreate(element, nameFilesPatterns, nameFilesObjects);
                    Elements.Add(element);
                }
            }
        }
        private ElementDB ConnectTemplateObjectAndCreate(ElementDB element, List<string> nameFilePatterns, List<string> nameFilesObjects)
        {
            if (IsHaveError(CheckError.ErrorNotHavePatterns(nameFilePatterns)))
            {
                foreach (string name in nameFilesObjects)
                {
                    int lastSymbol = name.LastIndexOf('\\') + 1;
                    string correctName = name.Substring(lastSymbol, name.LastIndexOf('.') - lastSymbol);
                    if(element.FullPattern.Pattern.Name.CompareTo(correctName) == 0)
                    {
                        element.Data = LoaderData.ReadObjectDB(name, element.FullPattern.Pattern);
                        State = LoaderData.State;
                        break;
                    }
                }
            }
            return element;
        }
        private bool IsHaveError(string state = null)
        {
            if (state != null)
                State = state;
            return State.CompareTo(CheckError.NotError) == 0;
        }
    }
}
