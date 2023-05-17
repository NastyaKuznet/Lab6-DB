using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Core0
{
    public static class CheckError
    {
        public static string NotError = "Нет ошибок.";
        public static string MismatchPattensAndObjects = "Нет схемы для метаданных.\nПроверьте названия файлов метаданных и схемы.";
        
        public static string ErrorInstallationForeignKey(bool isForeginKey, ObservableCollection<string> collectNameTables)
        {
            if (isForeginKey && collectNameTables.Count == 0)
                return "Нельзя устанавливать внешний ключ, пока не добавлены другие таблицы.";
            return NotError;
        }
        public static string ErrorNotHavePatterns(List<string> patterns)
        {
            if (patterns.Count == 0)
                return "В папке нет файлов схем.";
            return NotError;
        }
        public static string InputErrorRightCountColumns(string[] date, int count, string nameFile)
        {
            if (date.Length != count)
                return $"Ошибка количества столбцов в файле {nameFile.Substring(nameFile.LastIndexOf('\\') + 1)}.";
            return NotError;
        }
        public static string InputErrorInt(string num)
        {
            if (!int.TryParse(num, out int number))
                return "Введенное значение должно быть типа int.";
            return NotError;
        }
        public static string ErrorEmptyString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "Нужно заполнить все поля.";
            return NotError;
        }
        public static string ErrorEmptyStringWithForeignKey(bool isForeginKey, string str)
        {
            if (isForeginKey && string.IsNullOrEmpty(str))
                return "Нужно заполнить все поля.";
            return NotError;
        }

        private static string InputErrorIntInColumn(string num, string nameColumn)
        {
            if (!int.TryParse(num, out int number))
                return $"Ошибка типа данных в столбце {nameColumn}.";
            return NotError;
        }
        private static string InputErrorDateTimeInColumn(string num, string nameColumn)
        {
            if (!DateTime.TryParse(num, out DateTime dateTime))
                return $"Ошибка типа данных в столбце {nameColumn}.";
            return NotError;
        }
        public static string InputErrorProperties(string[] line, PatternObjectDB patternObjectDB)
        {
            string state = "";
            int i = 0;
            foreach (PatternPropertyDB prop in patternObjectDB.Properties.Values)
            {
                switch (prop.Type)
                {
                    case "int":
                        state = InputErrorIntInColumn(line[i], prop.Name);
                        break;
                    case "dateTime":
                        state = InputErrorDateTimeInColumn(line[i], prop.Name);
                        break;
                    case "string":
                        state = NotError;
                        break;
                    default:
                        state = "Не известный тип данных.";
                        break;
                }
                if (!state.Contains(NotError))
                    return state;
                i++;
            }
            return state;
        }
        public static string ErrorTypeColumnIsForeignKey(string nameColumn, bool isForeignKey, string type, Dictionary<string, Dictionary<string, string>> collectType, 
            string nameTable, string nameColumns)
        {
            if (isForeignKey && type.CompareTo(collectType[nameTable][nameColumns]) != 0)
                return $"Колонка {nameColumn} с внешним ключом должна быть с типом {collectType[nameTable][nameColumns]}.";
            return NotError;
        }
        public static string ErrorContentCellIsForeignKey(string content, bool isForeignKey, List<string> contentLinkColumn, string nameLinkTable, string nameLinkColumn)
        {
            if (isForeignKey && !contentLinkColumn.Contains(content))
                return $"Значение {content} не содержится в связанной \nтаблице {nameLinkTable} (столбец {nameLinkColumn}).";
            return NotError;
        }
        public static string ErrorUniquenessPrimaryKey(List<bool> statePrimaryKeys)
        {
            int count = 0;
            foreach(bool state in statePrimaryKeys)
            {
                if (state)
                    count++;
                if (count > 1)
                    return "В таблице должен быть один первичный ключ.";
            }
            return NotError;
        }
        public static string ErrorTypePrimaryKey(bool isPrimaryKey, string type)
        {
            if (isPrimaryKey && type.CompareTo("int") != 0)
                return "Первичный ключ должен быть типа int";
            return NotError;
        }
        public static string ErrorPrimaryKeyIsNotForeign(bool isPrimaryKey, bool isForeignKey)
        {
            if (isPrimaryKey && isForeignKey)
                return "Первичный ключ не может быть внешним";
            return NotError;
        }
        public static string ErrorOrderInColumnPrimary(bool isPrimaryKey, string contentCell, int rightOrder)
        {
            if (isPrimaryKey)
            {
                int contentCellInt = int.Parse(contentCell);
                if (contentCellInt != rightOrder)
                    return $"Несоответсвующий порядок в столбце с первичным ключом в ячейке {contentCell}.\nДолжен быть {rightOrder}";
            }
            return NotError;
        }
        public static string ErrorPrimaryKeyAndReferencingTables(string nameColumn, bool isPrimaryKey, Dictionary<string, PatternPropertyDB> referensing, string keyReferensing, bool isStillHaveReferensing)
        {
            if (!isPrimaryKey && referensing.ContainsKey(keyReferensing)&& referensing[keyReferensing].ReferencingTables != null && referensing[keyReferensing].ReferencingTables.Count != 0 && isStillHaveReferensing)
                return $"Нельзя снять с колонки {nameColumn} первинчый ключ, если на неё ссылаются другие таблицы.";
            return NotError;
        }

    }
}
