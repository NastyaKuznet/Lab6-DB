using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.RequestFolder
{
    public static class OperationGet
    {
        public static string State = CheckError.NotError;
        public static DataTable DoOperation(string[] elementsOperation, DataTable tableJoin)
        {
            DataTable tableGet = new DataTable();
            List<string> namesColumns = CreateListNeededNameColumn(elementsOperation);
            var tuple = AddNeededColumn(tableJoin, namesColumns);
            if (IsHaveError())
            {
                DataTable newTable = tuple.Item1;
                List<int> indexesColumns = tuple.Item2;
                tableGet = AddRows(newTable, indexesColumns, tableJoin);
                tableGet.TableName = "table";
            }
            return tableGet;
        }
        private static List<string> CreateListNeededNameColumn(string[] elementsOperation)
        {
            List<string> namesColumns = new List<string>();
            List<string> parthName = new List<string>();
            for (int i = 1; i < elementsOperation.Length; i++)
            {
                if (IsOneSet(elementsOperation[i]))
                    namesColumns.Add(CreateInCorrectFormatNameColumn(elementsOperation[i]));
                else
                {
                    parthName.Add(elementsOperation[i]);
                    if (IsCloseSet(elementsOperation[i]))
                    {
                        namesColumns = CreateCorrectName(parthName, namesColumns);
                    }
                }
            }
            return namesColumns;
        }
        private static string CreateInCorrectFormatNameColumn(string name)
        {
            string[] pathNewName = name.Trim(',').Split('(');
            return pathNewName[0] + "-" + pathNewName[1].Trim(')');
        }
        private static List<string> CreateCorrectName(List<string> pathName, List<string> nameColumns)
        {
            string nameTable = " ";
            foreach (string path in pathName)
            {
                if (IsOpenSet(path))
                {
                    string[] content = path.Split('(');
                    nameTable = content[0];
                    nameColumns.Add(CreateInCorrectFormatNameColumn(path).Trim(' '));
                }
                else
                {
                    nameColumns.Add(nameTable + "-" + path.Trim(new char[] { ',', ')' }));
                }
            }
            return nameColumns;
        }
        private static (DataTable, List<int>) AddNeededColumn(DataTable searchTable, List<string> namesColumns)
        {
            DataTable newTable = new DataTable();
            List<int> indexesColumns = new List<int>();
            int index = 0;
            int count = 0;
            foreach (DataColumn column in searchTable.Columns)
            {
                if (namesColumns.Contains(column.ColumnName))
                {
                    DataColumn newColumn = new DataColumn(column.ColumnName);
                    newTable.Columns.Add(newColumn);
                    indexesColumns.Add(index);
                    count++;
                }
                index++;
            }
            if (count != namesColumns.Count)
                State = "Не найдена нужная колонка.";
            return (newTable, indexesColumns);
        }
        private static DataTable AddRows(DataTable newTable, List<int> indexesColumn, DataTable tableJoin)
        {
            foreach (DataRow row in tableJoin.Rows)
            {
                newTable.Rows.Add(CreateNewRowByIndexesColumns(newTable, row, indexesColumn));
            }
            return newTable;
        }
        private static DataRow CreateNewRowByIndexesColumns(DataTable table, DataRow row, List<int> indexesColumn)
        {
            DataRow newRow = table.NewRow();
            int indexNewRow = 0;
            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                if (indexesColumn.Contains(i))
                {
                    newRow[indexNewRow] = row[i];
                    indexNewRow++;
                }
            }
            return newRow;
        }

        private static bool IsOneSet(string str)
        {
            return str.Contains('(') && str.Contains(")");
        }
        private static bool IsOpenSet(string str)
        {
            return str.Contains('(');
        }
        private static bool IsCloseSet(string str)
        {
            return str.Contains(")");
        }
        private static bool IsHaveError(string state = null)
        {
            if (state != null)
                State = state;
            return State.CompareTo(CheckError.NotError) == 0;
        }
    }
}
