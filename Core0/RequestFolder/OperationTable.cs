using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core0.RequestFolder
{
    public static class OperationTable
    {
        public static string State = CheckError.NotError;
        public static ObservableCollection<ElementDB> DoOperation(string[] elementsOperation, ObservableCollection<ElementDB> elements)
        {
            ObservableCollection<ElementDB> workElements = new ObservableCollection<ElementDB>();
            for (int i = 1; i < elementsOperation.Length; i++)
            {
                string elementOp = elementsOperation[i].Trim(',');
                ElementDB searhElement = Searher.SearchElementByName(elementOp, elements);
                State = Searher.State;
                if (!IsHaveError()) break;
                workElements.Add(searhElement);
            }
            return workElements;
        }

        private static bool IsHaveError(string state = null)
        {
            if (state != null)
                State = state;
            return State.CompareTo(CheckError.NotError) == 0;
        }
    }
}
