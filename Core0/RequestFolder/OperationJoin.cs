using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.RequestFolder
{
    public static class OperationJoin
    {
        public static string State = CheckError.NotError;
        public static DataTable DoOperation(string[] elementsOperation, DataTable tableJoin, ObservableCollection<ElementDB> workElements)
        {
            DataTable resultTable = new DataTable();
            if (IsHaveError(CheckSpecialSymbolJOIN(elementsOperation)))
            {
                string baseName = elementsOperation[1];
                string subName = elementsOperation[3];
                string[] connect = elementsOperation[5].Split('=');
                string baseCondition = tableJoin == null ? connect[0].Split('.')[1] : string.Join('-', connect[0].Split('.'));
                string subCondition = connect[1].Split('.')[1];

                DataTable baseTable = tableJoin == null ? Searher.SearchElementByName(baseName, workElements).Table : tableJoin;
                DataTable subTable = Searher.SearchElementByName(subName, workElements).Table;
                State = Searher.State;

                if (IsHaveError())
                {
                    int baseIndexColumn = Searher.SearchIndexColumnByName(baseCondition, baseTable);
                    int subIndexColumn = Searher.SearchIndexColumnByName(subCondition, subTable);
                    State = Searher.State;
                    if (IsHaveError(CheckContentColumnInTable(baseIndexColumn)) && IsHaveError(CheckContentColumnInTable(subIndexColumn)))
                        resultTable = CreateNewTableFromJoin(baseTable, subTable, baseIndexColumn, subIndexColumn, tableJoin);
                }
            }
            return resultTable;
        }
        private static DataTable CreateNewTableFromJoin(DataTable baseTable, DataTable subTable, int baseIndexColumn, int subIndexColumn, DataTable tableJoin)
        {
            DataTable newTable = ConnectColumnTable(baseTable, subTable, tableJoin);
            newTable = CreateNewRow(baseTable, subTable, baseIndexColumn, subIndexColumn, newTable);
            return newTable;
        }
        private static DataTable ConnectColumnTable(DataTable baseTable, DataTable subTable, DataTable tableJoin)
        {
            DataTable newTable = new DataTable();
            newTable = AddColimnsFromDifferentTable(newTable, baseTable, tableJoin);
            newTable = AddColimnsFromDifferentTable(newTable, subTable, tableJoin);
            return newTable;
        }
        private static DataTable AddColimnsFromDifferentTable(DataTable baseTable, DataTable diffTable, DataTable tableJoin)
        {
            foreach (DataColumn column in diffTable.Columns)
            {
                DataColumn newColumn = new DataColumn();
                if (diffTable != tableJoin)
                    newColumn.ColumnName = diffTable.TableName + "-" + column.ColumnName;
                else
                    newColumn.ColumnName = column.ColumnName;
                baseTable.Columns.Add(newColumn);
            }
            return baseTable;
        }
        private static DataTable CreateNewRow(DataTable baseTable, DataTable subTable, int baseIndexColumn, int subIndexColumn, DataTable newTable)
        {
            foreach (DataRow baseRow in baseTable.Rows)
            {
                DataRow subRow = Searher.SearchRowByCondition(baseRow[baseIndexColumn].ToString(), subTable, subIndexColumn);
                DataRow newRow = AddNewRow(baseRow, subRow, newTable);
                newTable.Rows.Add(newRow);
            }
            return newTable;
        }
        private static DataRow AddNewRow(DataRow baseRow, DataRow subRow, DataTable newTable)
        {
            DataRow newRow = newTable.NewRow();
            newRow = AddCellFromDifferentRow(newRow, baseRow);
            int indexAdd = baseRow.ItemArray.Length;
            newRow = AddCellFromDifferentRow(newRow, subRow, indexAdd);
            return newRow;
        }
        private static DataRow AddCellFromDifferentRow(DataRow newRow, DataRow diffRow, int indexAdd = 0)
        {
            for (int i = indexAdd; i < diffRow.ItemArray.Length + indexAdd; i++)
            {
                newRow[i] = diffRow.ItemArray[i - indexAdd];
            }
            return newRow;
        }

        private static string CheckSpecialSymbolJOIN(string[] str)
        {
            if (str[2].CompareTo("->") != 0 || str[4].CompareTo("on") != 0 || !str[5].Contains('='))
                return "Не хватает специальных символов => и(или) on и(или) = в операции JOIN.";
            return CheckError.NotError;
        }
        private static string CheckContentColumnInTable(int index = -1)
        {
            if (index == -1)
                return "Не найдена нужная колонка.";
            return CheckError.NotError;
        }
        private static bool IsHaveError(string state = null)
        {
            if (state != null)
                State = state;
            return State.CompareTo(CheckError.NotError) == 0;
        }

    }
}
