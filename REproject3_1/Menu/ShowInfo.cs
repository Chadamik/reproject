using REproject3_1.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REproject3_1.Menu
{
    /// <summary>
    /// Класс для выполнения основной задачи.
    /// </summary>
    public static class ShowInfo
    {
        /// <summary>
        /// Четвертый пункт меню(основная задача), информация о посетителе по id.
        /// В условиях задачи не было сказано, выводить информацию о все посетителях с совпадающим id.
        /// Или достаточно вывести информацию хотя бы об одном посетителе с найденным id.
        /// Тем не менее, было бы странно, если на вход мы бы получили данные, где у нескольких посетитилей совпадают id.
        /// Поэтому я посчитал, что такого случая не существует и вывел информацию о первом найденном посетителе с заданным id.
        /// </summary>
        /// <param name="visitors"></param>
        public static void Dialog(List<Visitor> visitors)
        {
            Console.Clear();
            Console.Write("Введите id посетителя    ");

            string id = Console.ReadLine() ?? "";
            Console.Clear();

            foreach (Visitor visitor in visitors)
            {
                if (visitor.GetField("id") == id)
                {
                    Console.WriteLine(visitor.GetStringForMainTask());
                    return;
                }
            }
            Console.WriteLine("Посетитель с таким id не найден");
        }
    }
}
