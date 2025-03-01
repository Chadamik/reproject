using REproject3_1.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REproject3_1.Menu
{
    /// <summary>
    /// Класс для сортировки посетителей.
    /// </summary>
    public static class SortData
    {
        /// <summary>
        /// Диаалог с пользователем для сортировки.
        /// </summary>
        /// <param name="visitors"></param>
        public static void Dialog(List<Visitor> visitors)
        {

            Console.Clear();
            Console.WriteLine("Доступные поля для сортировки:");
            Console.WriteLine("1.id");
            Console.WriteLine("2.label");
            Console.WriteLine("3.inherits");
            Console.WriteLine("4.decayto");
            Console.WriteLine("5.icon");
            Console.WriteLine("6.lifetime");
            Console.Write("Введите число от 1 до 6: ");

            string fieldToSort = ChooseField();

            Console.WriteLine("Введите направление сортировки");
            Console.WriteLine("1.По возрастанию");
            Console.WriteLine("2.По убыванию");
            Console.Write("Введите 1 или 2: ");

            if (int.TryParse(Console.ReadLine(), out int secondchoice))
            {
                try
                {
                    SortVisitors(visitors, fieldToSort, secondchoice);

                    Console.WriteLine("Сортировка завершена.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка при сортировке: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Ошибка, неверный ввод");
            }
        }
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
        /// Сортировка.
        /// </summary>
        /// <param name="visitors"></param>
        /// <param name="fieldToSort"></param>
        /// <param name="secondChoice"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void SortVisitors(List<Visitor> visitors, string fieldToSort, int secondChoice)
        {
            switch (secondChoice)
            {
                case 1: //по возрастанию
                    List<Visitor> sortVisitors = fieldToSort == "lifetime" ? ([.. visitors.OrderBy(d => d.GetField("lifetime"))]) : ([.. visitors.OrderBy(d => d.GetField(fieldToSort))]);
                    visitors.Clear();
                    visitors.AddRange(sortVisitors);
                    break;
                case 2: //по убыванию
                    List<Visitor> sortedVisitors = fieldToSort == "lifetime" ? ([.. visitors.OrderByDescending(d => d.GetField("lifetime"))]) : ([.. visitors.OrderByDescending(d => d.GetField(fieldToSort))]);
                    visitors.Clear();
                    visitors.AddRange(sortedVisitors);
                    break;
                default:
                    Console.WriteLine("Ошибка, неверный выбор");
                    throw new ArgumentException("Неверный ввод");

            }
        }
    }
}
