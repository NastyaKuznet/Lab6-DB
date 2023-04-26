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

        private static string InputErrorInt(string num, string nameColumn)
        {
            if (!int.TryParse(num, out int number))
                return $"Ошибка типа данных в столбце {nameColumn}.";
            return NotError;
        }
        private static string InputErrorDateTime(string num, string nameColumn)
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
                    case PatternPropertyDB.PropertyType.Int:
                        state = InputErrorInt(line[i], prop.Name);
                        break;
                    case PatternPropertyDB.PropertyType.DataTime:
                        state = InputErrorDateTime(line[i], prop.Name);
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
