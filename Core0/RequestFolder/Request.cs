using Core0;
using Core0.RequestFolder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7DB.ViewModel
{
    public class Request
    {
        private ObservableCollection<ElementDB> elements;
        private ObservableCollection<ElementDB> workElements = new ObservableCollection<ElementDB>();
        private DataTable tableJoin;
        public DataTable TableGet { get; private set; }
        public string State { get; set; }
        public Request(string textRequest, ObservableCollection<ElementDB> elementDBs) 
        {
            State = CheckError.NotError;
            elements = elementDBs;

            ProcessRequest(textRequest);
        }

        private void ProcessRequest(string textRequest) 
        {
            string[] operations = textRequest.Split('\n');
            foreach (string operation in operations) 
            {
                if (!IsHaveError()) break;
                string[] elementsOperation = operation.Split(' ');
                if(!string.IsNullOrEmpty(operation))
                    NavigateCommand(elementsOperation);
            }
        }
        private void NavigateCommand(string[] elementsOperation)
        {
            switch(elementsOperation[0])
            {
                case "TABLES":
                    workElements = OperationTable.DoOperation(elementsOperation, elements);
                    State = OperationTable.State;
                    break;
                case "JOIN":
                    tableJoin = OperationJoin.DoOperation(elementsOperation, tableJoin, workElements);
                    State = OperationJoin.State;
                    break;
                case "GET":
                    TableGet = OperationGet.DoOperation(elementsOperation, tableJoin);
                    State = OperationGet.State;
                    break;
                default:
                    State = "Неизвестная операция";
                    break;
            }
        }

        private bool IsHaveError(string state = null)
        {
            if (state != null)
                State = state;
            return State.CompareTo(CheckError.NotError) == 0;
        }
    }
}
