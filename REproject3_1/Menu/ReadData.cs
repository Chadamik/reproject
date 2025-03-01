using JSONLibrary;
using REproject3_1.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REproject3_1.Menu
{
    /// <summary>
    /// Класс для чтения данных.
    /// </summary>
    public static class ReadData
    {
        /// <summary>
        /// Диалог для чтения данных.
        /// </summary>
        /// <param name="visitors"></param>
        public static void Dialog(List<Visitor> visitors)
        {
            Console.Clear();
            Console.WriteLine("Каким путем хотите ввести данные?");
            Console.WriteLine("1.Консоль");
            Console.WriteLine("2.Файл");
            Console.Write("Введите 1 или 2: ");
            string stringChoice = Console.ReadLine() ?? "";
            switch (stringChoice)
            {
                case "1": // Чтение с консоли
                    try
                    {
                        ReadFromConsole(visitors);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Ошибка при вводе данных: {e.Message}");
                    }
                    break;
                case "2": //Чтение с файла
                    try
                    {
                        ReadFromFile(visitors);
                    }
                    catch (Exception e)
                    {
                        Console.SetIn(new StreamReader(Console.OpenStandardInput()));
                        Console.WriteLine($"Ошибка при чтении файла: {e.Message}");
                    }
                    break;
                default:
                    Console.WriteLine("Неверный ввод");
                    break;
            }
        }

        /// <summary>
        /// Чтение json-данных с консоли.
        /// </summary>
        /// <param name="visitors"></param>
        private static void ReadFromConsole(List<Visitor> visitors)
        {
            Console.WriteLine("Введите json-данные:(Введите \"banana\" без кавычек для завершения)");

            // Cписок с visitors, представленными в виде строк
            List<string> elements = JsonParser.ReadJson(Console.In);

            foreach (string element in elements)
            {
                visitors.Add(new(element));
            }

            if (visitors.Count != 0) { Console.WriteLine("Данные успешно загружены из консоли."); }
            else { Console.WriteLine("В файле недостаточно данных"); }
        }

        /// <summary>
        /// Чтение json-данных с файла.
        /// </summary>
        /// <param name="visitors"></param>
        /// <exception cref="Exception"></exception>
        private static void ReadFromFile(List<Visitor> visitors)
        {
            Console.Write("Введите название файла относительно папки проекта(файл должен лежать в rewriteProjectBanana) ");
            string path = UtilsClass.GetDirectory() + Console.ReadLine() ?? "";
            if (!File.Exists(path)) { throw new Exception("Файла не существует"); }

            // Перенаправляем стандартный ввод на файл
            TextReader reader = Console.In;
            using StreamReader fileReader = new(path);
            Console.SetIn(fileReader);

            // Cписок с visitors, представленными в виде строк
            List<string> elements = JsonParser.ReadJson(reader);

            foreach (string element in elements)
            {
                visitors.Add(new(element));
            }

            // Восстанавливаем стандартный ввод
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            Console.Clear();
            if (visitors.Count != 0) { Console.WriteLine("Данные успешно загружены из консоли."); }
            else { Console.WriteLine("В файле недостаточно данных"); }
        }
    }
}
