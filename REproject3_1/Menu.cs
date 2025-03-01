using JSONLibrary;
using REproject3_1.Objects;
using ClosedXML.Excel;
using System.Text;
using REproject3_1.Menu;

namespace REproject3_1
{
    /// <summary>
    /// Интерактивный интерфейс для работы с коллекцией объектов.
    /// </summary>
    public class MenuOptions
    {
        private readonly List<Visitor> visitors;

        public MenuOptions()
        {
            visitors = [];
        }

        /// <summary>
        /// Класс с заголовками для интерфейса.
        /// </summary>
        public void Dialog()
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Количество загруженных данных: {visitors.Count}");
                Console.WriteLine("1.Ввести данные (консоль/файл)");
                Console.WriteLine("2.Отфильтровать данные");
                Console.WriteLine("3.Отсортировать данные");
                Console.WriteLine("4.Вывести информацию о посетителе");
                Console.WriteLine("5.Ввести/вывести данные в виде excel-таблицы");
                Console.WriteLine("6.Вывести данные (консоль/файл)");
                Console.WriteLine("7.Выход");
                Console.Write("Введите число от 1 до 7: ");

                try
                {
                    if (int.TryParse(Console.ReadLine(), out int result))
                    {
                        switch (result)
                        {
                            case 1:
                                ReadData.Dialog(visitors);
                                break;
                            case 2:
                                FiltData.Dialog(visitors);
                                break;
                            case 3:
                                SortData.Dialog(visitors);
                                break;
                            case 4:
                                ShowInfo.Dialog(visitors);
                                break;
                            case 5:
                                ExcelWork.Dialog(visitors);
                                break;
                            case 6:
                                OutputData.Dialog(visitors);
                                break;
                            case 7:
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Ошибка, неверный ввод, попробуйте снова");
                                break;
                        };
                    }
                    else
                    {
                        Console.WriteLine("Ошибка, неверный ввод");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка: {e.Message}");
                }

                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                _ = Console.ReadKey();
            }
        }
    }
}
