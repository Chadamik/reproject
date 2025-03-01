using System;
using System.Text;

namespace JSONLibrary
{
    /// <summary>
    /// Статический класс для парсинга json-объектов.
    /// Класс работает с интерфейсом IJSONObject.
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// Чтение json объекта.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<string> ReadJson(TextReader reader) 
        {
            string json;
            if (reader == Console.In)
            {
                StringBuilder stringBuilder = new();
                while (true)
                {
                    string? line = Console.ReadLine();
                    if (line == "banana") { break; }
                    _ = stringBuilder.Append(line);
                }
                json = stringBuilder.ToString();
            }
            else
            {
                json = Console.In.ReadToEnd();
            }

            // Cловарь для представления полей json объекта
            Dictionary<string, string> dictionary = ParseJsonToDictionary(json);

            List<string> elements = [];

            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                elements.AddRange(SplitArray(pair.Value));
            }

            return elements;
        }

        /// <summary>
        /// Основный метод класса, представление полей объекта в виде словаря.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Dictionary<string, string> ParseJsonToDictionary(string jsonString)
        {
            // Удаляем лишние пробелы
            jsonString = RemoveWhiteSpaces(jsonString);

            // Первая проверка на корректность
            if (jsonString[0] != '{' || jsonString[^1] != '}') { throw new ArgumentException("Неверный первый или последний символ json"); }

            Dictionary<string, string> dictionary = [];
            int i = 1, length = jsonString.Length - 1;

            // Пробегаемся по строке и добавляем поля в словарь
            while (i < length)
            {
                if (jsonString[i] == '}') { break; }

                // Находим ключ
                string key = ReadString(jsonString, ref i);

                if (jsonString[i++] != ':')
                {
                    throw new ArgumentException("Пропущено двоеточие между ключом и значением");
                }

                // Находим значение
                string value = ReadValue(jsonString, ref i);

                // Добавляем в словарь
                dictionary[key] = value;

                if (jsonString[i] == ',')
                {
                    i++;
                }
            }

            return dictionary;
        }
        
        /// <summary>
        /// Метод извлечения строки-ключа или строки-значения
        /// </summary>
        /// <param name="json"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static string ReadString(string json, ref int index)
        {
            // Подразумевается, что все лишние пробелы удалены
            if (json[index++] != '"')
            {
                throw new ArgumentException("Invalid JSON format");
            }

            StringBuilder sb = new();
            while (index < json.Length)
            {
                // Важный случай, когда кавычки находятся внутри кавычек
                if (json[index] == '\\' && index + 1 < json.Length && json[index + 1] == '"')
                {
                    _ = sb.Append('\\');
                    _ = sb.Append('"');
                    index += 2;
                }
                else if (json[index] == '"')
                {
                    index++;
                    return sb.ToString();
                }
                else
                {
                    _ = sb.Append(json[index]);
                    index++;
                }
            }

            throw new ArgumentException("Ошибка в выражении в кавычках");
        }
        /// <summary>
        /// Метод извлечения числа-значения
        /// </summary>
        /// <param name="json"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static string ReadInt(string json, ref int index)
        {
            StringBuilder sb = new();

            while (index < json.Length)
            {
                // Конец чтения строки
                if (json[index] is ',' or '}')
                {
                    return !int.TryParse(sb.ToString(), out _) ? throw new ArgumentException("Ошибка при чтении значения-числа") : sb.ToString();
                }

                // Строка не закончилась
                _ = sb.Append(json[index]);
                index++;
            }

            throw new ArgumentException("Ошибка при чтении значения-числа");
        }

        /// <summary>
        /// Извлечение значения(для словаря)
        /// </summary>
        /// <param name="json"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string ReadValue(string json, ref int index)
        {
            if (json[index] is '{' or '[')
            {
                // Случай массива или сложного объекта
                int start = index;
                int braceCount = 0;
                do
                {
                    if (json[index] is '{' or '[')
                    {
                        braceCount++;
                    }

                    if (json[index] is '}' or ']')
                    {
                        braceCount--;
                    }

                    index++;
                } while (braceCount > 0 && index < json.Length);

                return json[start..index];
            }
            else
            {
                // Случай строки или числа
                return json[index] == '"' ? ReadString(json, ref index) : ReadInt(json, ref index);
            }
        }

        /// <summary>
        /// Удаляет все пробелы(кроме пробелов в кавычках)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static string RemoveWhiteSpaces(string json)
        {
            StringBuilder stringBuilder = new();
            bool insideQuotes = false;

            foreach(char symbol in json) 
            {
                if (symbol == '"')
                {
                    insideQuotes = !insideQuotes;
                }

                if (!insideQuotes)
                {
                    if (!char.IsWhiteSpace(symbol))
                    {
                        _ = stringBuilder.Append(symbol);
                    }
                }
                else
                {
                    _ = stringBuilder.Append(symbol);
                }

            }

            return stringBuilder.ToString();
        }
        /// <summary>
        /// Разделяет массив, представленный в виде строки на строки-объекты.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<string> SplitArray(string json)
        {
            json = json.Trim('[', ']');

            List<string> elements = [];
            int start = 0, finish;
            int leftBracket = 0, rightBracket = 0;
            // Находимся внутри объекта или нет
            bool inObject = false;
            // Находимся до запятой или после
            bool objectWasAdded = false;
            for (int i = 0; i < json.Length; i++)
            {
                if (json[i] == '{')
                {
                    leftBracket++;
                    if (leftBracket == rightBracket + 1 && !inObject)
                    {
                        start = i;
                        inObject = true;
                    }
                }

                if (json[i] == '}')
                {
                    rightBracket++;
                    if (leftBracket == rightBracket)
                    {
                        finish = i + 1;
                        inObject = false;
                        elements.Add(json[start..finish]);
                        objectWasAdded = true;
                    }
                }

                if (json[i] == ',' && !inObject)
                {
                    if(objectWasAdded)
                    {
                        objectWasAdded = false;
                        start = i + 1;
                    }
                    else
                    {
                        finish = i;
                        elements.Add(json[start..finish]);
                        objectWasAdded = true;
                    }
                }
            }
            return elements;
        }

        /// <summary>
        /// Метод, выводящий данные списка класса, реализующего IJSONObject в формате json.
        /// </summary>
        /// <param name="visitors"></param>
        public static void WriteJson<T>(List<T> visitors) where T : IJSONObject
        {
            StringBuilder sb = new();

            _ = sb.Append("{\"elements\":[");

            foreach (T visitor in visitors)
            {
                _ = sb.Append($"\n{{\n{visitor}\n}},");
            }

            _ = sb.Remove(sb.Length - 1, 1);
            _ = sb.Append("]}");

            Console.WriteLine(sb.ToString());
        }
    }
}
