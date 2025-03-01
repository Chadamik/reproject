using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONLibrary
{
    /// <summary>
    /// Нестатический класс для формирования объектов в виде json строк.
    /// </summary>
    public class FormatterJsonStrings
    {
        private readonly IJSONObject? _jsonObject;

        /// <summary>
        /// Конструктор с пустыми параметрами.
        /// </summary>
        public FormatterJsonStrings()
        {
            _jsonObject = null;
        }

        /// <summary>
        /// Конструктор с параметром - объекта с типом, реализующего IJSONObject.
        /// </summary>
        /// <param name="jsonObject"></param>
        public FormatterJsonStrings(IJSONObject? jsonObject)
        {
            _jsonObject = jsonObject;
        }

        /// <summary>
        /// Представление поля-строки в формате json.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetStringFieldToJson(string fieldName)
        {
            string fieldValue = _jsonObject !=null ? _jsonObject.GetField(fieldName) ?? "" : "";
            return (fieldValue.StartsWith('{') || (fieldValue.StartsWith('[') && fieldName != "desc"))
                ? $"\"{fieldName}\":{fieldValue},\n"
                : fieldValue != "" ? $"\"{fieldName}\":\"{fieldValue}\",\n" : "";
        }

        /// <summary>
        /// Представление поля-числа в виде json.
        /// </summary>
        /// <returns></returns>
        public string GetIntToJson(string fieldName)
        {
            string fieldValue = _jsonObject != null ? _jsonObject.GetField(fieldName) ?? "" : "";
            return fieldValue == "^"
                ? $"\"{fieldName}\":\"^\",\n"
                : fieldValue == "" ? "" : $"\"{fieldName}\":{fieldValue},\n";
        }

        /// <summary>
        /// Удаляет лишние запятые json строки.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string RemoveExtraComma(string json)
        {
            if (json.Length >= 2)
            {
                if (json[^1] == ',')
                {
                    json = json.Remove(json.Length - 1, 1);
                }
                if (json[^1] == '\n' && json[^2] == ',')
                {
                    json = json.Remove(json.Length - 2, 2);
                }
            }

            return json;
        }

        /// <summary>
        /// Запись словаря в виде json-строки.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public string DictionaryToString(Dictionary<string, string> dictionary)
        {
            if (dictionary.Count == 0) { return ""; }
            StringBuilder sb = new();
            _ = sb.Append('{');
            foreach (string key in dictionary.Keys)
            {
                _ = sb.Append($"\"{key}\":\"{dictionary[key]},");
            }
            _ = sb.Remove(sb.Length - 1, 1);
            _ = sb.Append('}');
            return sb.ToString();
        }
    }
}
