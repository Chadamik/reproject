using REproject3_1.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REproject3_1.Menu
{
    /// <summary>
    /// Статический класс для фильтрации списка.
    /// </summary>
    public static class FiltData
    {
        /// <summary>
        /// Диалог для фильтрации.
        /// </summary>
        /// <param name="visitors"></param>
        public static void Dialog(List<Visitor> visitors)
        {
            Console.Clear();
            Console.WriteLine("Доступные поля для фильтрации:");
            Console.WriteLine("1.id");
            Console.WriteLine("2.label");
            Console.WriteLine("3.inherits");
            Console.WriteLine("4.decayto");
            Console.WriteLine("5.icon");
            Console.WriteLine("6.lifetime");
            Console.Write("Введите число от 1 до 6: ");
            try
            {
                string fieldToFilter = ChooseField();
                Console.WriteLine("Введите множество значений без кавычек для поля через пробел");
                string[] values = (Console.ReadLine() ?? "").Split();

                List<Visitor> newVisitors = FiltVisitors(visitors, fieldToFilter, values);

                visitors.Clear();
                visitors.AddRange(newVisitors);
                Console.WriteLine("Фильтрация успешно завершена");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при фильтрации: {e.Message}");
            }
        }

        /// <summary>
        /// Выбор поля для фильтрации.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static string ChooseField()
        {
            string choice = Console.ReadLine() ?? "";
            string fieldToFilter = choice switch
            {
                "1" => "id",
                "2" => "label",
                "3" => "inherits",
                "4" => "decayto",
                "5" => "icon",
                "6" => "lifetime",
                _ => throw new ArgumentException("Неверный ввод"),
            };
            return fieldToFilter;
        }

        /// <summary>
        /// Фильтрация списка Visitors.
        /// </summary>
        /// <param name="visitors"></param>
        /// <param name="fieldToFilter"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static List<Visitor> FiltVisitors(List<Visitor> visitors, string fieldToFilter, string[] values)
        {
            List<Visitor> newVisitors = [];

            foreach (string value in values)
            {
                foreach (Visitor visitor in visitors)
                {
                    if (visitor.GetField(fieldToFilter) == value)
                    {
                        newVisitors.Add(visitor);
                    }
                }
            }

            return newVisitors;
        }
    }
}
