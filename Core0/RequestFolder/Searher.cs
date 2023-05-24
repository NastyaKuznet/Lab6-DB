using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.RequestFolder
{
    public static class Searher
    {
        public static string State = CheckError.NotError;
        public static ElementDB SearchElementByName(string nameElement, ObservableCollection<ElementDB> _elements)
        {
            foreach (ElementDB element in _elements)
            {
                if (element.FullPattern.Pattern.Name.CompareTo(nameElement) == 0)
                {
                    return element;
                }
            }
            State = $"Нет таблицы {nameElement}";
            return null;
        }
        public static int SearchIndexColumnByName(string nameColumn, DataTable table)
        {
            int index = 0;
            foreach (DataColumn columnBase in table.Columns)
            {
                if (columnBase.ColumnName.CompareTo(nameColumn) == 0)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
        public static DataRow SearchRowByCondition(string contentCell, DataTable table, int indexColumn)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row[indexColumn].ToString().CompareTo(contentCell) == 0)
                {
                    return row;
                }
            }
            return null;
        }
    }
}
