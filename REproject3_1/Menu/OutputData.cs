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
    /// Cтатический класс для вывода данных.
    /// </summary>
    public static class OutputData
    {
        /// <summary>
        /// Шестой пункт меню, вывод данных.
        /// </summary>
        /// <param name="visitors"></param>
        public static void Dialog(List<Visitor> visitors)
        {
            if (visitors.Count == 0)
            {
                Console.WriteLine("Данных недостаточно для вывода");
                return;
            }
            Console.Clear();
            Console.WriteLine("Каким путем хотите вывести данные?");
            Console.WriteLine("1.Консоль");
            Console.WriteLine("2.Файл");
            Console.Write("Введите 1 или 2: ");
            string stringChoice = Console.ReadLine() ?? "";
            switch (stringChoice)
            {
                case "1": // Консоль
                    JsonParser.WriteJson(visitors);
                    break;
                case "2": // Файл
                    try
                    {
                        Console.Write("Введите название файла(файл будет записан в папку проекта)   ");
                        string filePath = UtilsClass.GetDirectory() + Console.ReadLine() ?? "";

                        // Перенаправляем стандартный вывод в файл
                        using (StreamWriter fileWriter = new(filePath))
                        {
                            Console.SetOut(fileWriter);
                            JsonParser.WriteJson(visitors);
                        }

                        // Восстанавливаем стандартный вывод
                        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.WriteLine("Данные успешно записаны в файл");
                    }
                    catch (Exception e)
                    {
                        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.WriteLine($"Ошибка при записи данных в файл: {e.Message}");
                    }
                    break;
                default:
                    Console.WriteLine("Неверный ввод");
                    break;
            }
        }
    }
}
