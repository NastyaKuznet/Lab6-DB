using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Lab6DB.Model;

namespace Lab6DB.Model
{
    static class CheckError
    {
        public static string NotError = "Нет ошибок.";
        public static string MismatchPattensAndObjects = "Нет схемы для метаданных.\nПроверьте названия файлов метаданных и схемы.";

        public static string IsNotHavePatterns(List<string> patterns)
        {
            if (patterns.Count == 0)
                return "В папке нет файлов схем.";
            return NotError;
        }
        public static string IsNotHaveObjects(List<string> patterns)
        {
            if (patterns.Count == 0)
                return "В папке нет файлов метаданных.\nПроверьте расширение (.csv).";
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
            if (String.IsNullOrEmpty(str))
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
                    case "datetime":
                        state = InputErrorDateTimeInColumn(line[i], prop.Name);
                        break;
                    case "string":
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

    }
}
