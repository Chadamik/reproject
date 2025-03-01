using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using REproject3_1.Objects;
using System.Threading.Tasks;
using ClosedXML.Excel;
using JSONLibrary;

namespace REproject3_1.Menu
{
    /// <summary>
    /// Статический класс для фильтрации 
    /// </summary>
    public static class ExcelWork
    {
        /// <summary>
        /// Пункт 5 в меню, дополнительная задача.
        /// Выбор - ввод или вывод excel файла.
        /// </summary>
        /// <param name="visitors"></param>
        public static void Dialog(List<Visitor> visitors)
        {
            Console.Clear();
            Console.WriteLine("1.Прочитать данные с excel файла");
            Console.WriteLine("2.Вывести данные в excel файл");

            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
            {
                InputExcelData(visitors);
            }
            else if (choice == "2")
            {
                OutputExcelData(visitors);
            }
            else
            {
                Console.WriteLine("Ошибка, неверный ввод");
            }
        }

        /// <summary>
        /// Чтение данных с excel файла.
        /// </summary>
        /// <param name="visitors"></param>
        private static void InputExcelData(List<Visitor> visitors)
        {
            Console.Write("Введите путь к файлу(относительно папки проекта)");

            string path = UtilsClass.GetDirectory() + Console.ReadLine() ?? "";

            // Список для хранения объектов
            List<Visitor> newVisitors = [];

            // Открытие Excel файла
            using XLWorkbook workbook = new(path);
            IXLWorksheet worksheet = workbook.Worksheet("Items");

            // Определение количества строк и столбцов
            IXLRows rows = worksheet.RowsUsed();

            // Здесь и далее numX - номер столбца X
            // Если X - словарь или массив, то numX - крайний левый столбец с информацией о X
            int numAspects = 10;
            int numXexts = 10;

            // Вычисляем сколько столбцов занимают словари и массивы
            while (worksheet.Cell(1, numXexts).GetString().StartsWith("Aspect"))
            {
                numXexts += 2;
            }

            int numSatisfyingInfo = numXexts;

            while (worksheet.Cell(1, numSatisfyingInfo).GetString().StartsWith("Xext"))
            {
                numSatisfyingInfo += 2;
            }

            int numSatisfyingId = numSatisfyingInfo + 1;
            int numSatisfyingMorpheffect = numSatisfyingId + 1;
            int numSatisfyingLevel = numSatisfyingMorpheffect + 1;
            int numAppointingId = numSatisfyingLevel + 1;
            int numAppointingMorpheffect = numAppointingId + 1;
            int numAppointingLevel = numAppointingMorpheffect + 1;
            int numDissatisfying = numAppointingLevel + 1;
            int numAdditionalInfo = numDissatisfying + 1;
            int numAdditionalLabels = numAdditionalInfo;
            int end = worksheet.ColumnsUsed().Count();

            while (worksheet.Cell(1, numAdditionalLabels).GetString().StartsWith("AdditionalInfo"))
            {
                numAdditionalLabels += 4;
            }
            // Чтение данных, начиная со второй строки
            foreach (IXLRow? row in rows.Skip(1))
            {
                Visitor visitor = new();

                // Добавление полей-строк
                for (int i = 1; i < numAspects; i++)
                {
                    visitor.SetField(worksheet.Cell(1, i).GetString().ToLower(), row.Cell(i).Value.ToString());
                }

                // Добавляем аспекты
                Dictionary<string, string> aspects = [];
                FormatterJsonStrings formatter = new(visitor);
                for (int i = numAspects; i < numXexts; i += 2)
                {
                    string aspectKey = row.Cell(i).Value.ToString();
                    string aspectValue = row.Cell(i + 1).Value.ToString();

                    if (aspectKey != "") { aspects.Add(aspectKey, aspectValue); }
                }
                if (aspects.Count != 0)
                {
                    visitor.SetField("aspects", formatter.DictionaryToString(aspects));
                }

                // Добавляем xexts
                Dictionary<string, string> xexts = [];
                for (int i = numXexts; i < numSatisfyingInfo; i += 2)
                {
                    string xextKey = row.Cell(i).Value.ToString();
                    string xextValue = row.Cell(i + 1).Value.ToString();

                    if (xextKey != "") { xexts.Add(xextKey, xextValue); }
                }
                if (xexts.Count != 0)
                {
                    visitor.SetField("xexts", formatter.DictionaryToString(xexts));
                }

                // Добавление xtriggers, из-за отсутсвия возможности записи сложных полей в visitor
                // И из-за сложной структуры поля xtriggers
                // Мы вынуждены конвертировать данные сначала в json, затем при инициализации xtriggers парсить обратно
                string satisfyingLabel = row.Cell(numSatisfyingInfo).Value.ToString();
                string satisfyingId = row.Cell(numSatisfyingId).Value.ToString();
                string satisfyingMorpheffect = row.Cell(numSatisfyingMorpheffect).Value.ToString();
                string satisfyingLevel = row.Cell(numSatisfyingLevel).Value.ToString();
                string appointingId = row.Cell(numAppointingId).Value.ToString();
                string appointingMorpheffect = row.Cell(numAppointingMorpheffect).Value.ToString();
                string appointingLevel = row.Cell(numAppointingLevel).Value.ToString();
                string dissatisfying = row.Cell(numDissatisfying).Value.ToString();

                StringBuilder sb = new();
                _ = sb.Append('{');
                _ = sb.Append($"\"satisfying\":[\"{satisfyingLabel}\",{{\"id\":\"{satisfyingId}\"," +
                                             $"\"morpheffect\":\"{satisfyingMorpheffect}\", \"level\":\"{satisfyingLevel}\"}}],");
                _ = sb.Append($"\"appointing\":[{{\"id\":\"{appointingId}\"," +
                                             $"\"morpheffect\":\"{appointingMorpheffect}\", \"level\":\"{appointingLevel}\"}}],");
                _ = sb.Append($"\"dissatisfying\":\"{dissatisfying}\",");
                for (int i = numAdditionalInfo; i < numAdditionalLabels; i += 4)
                {
                    string addKey = row.Cell(i).Value.ToString();
                    string addId = row.Cell(i + 1).Value.ToString();
                    string addMorpheffect = row.Cell(i + 2).Value.ToString();
                    string addLevel = row.Cell(i + 3).Value.ToString();

                    if (addKey != "")
                    {
                        _ = sb.Append($"\"{addKey}\":[{{\"id\":\"{addId}\"," +
                                             $"\"morpheffect\":\"{addMorpheffect}\", \"level\":\"{addLevel}\"}}],");
                    }
                }

                for (int i = numAdditionalLabels; i < end; i += 2)
                {
                    string addKey = row.Cell(i).Value.ToString();
                    string addLabel = row.Cell(i + 1).Value.ToString();

                    if (addKey != "")
                    {
                        _ = sb.Append($"\"{addKey}\":\"{addLabel}\",");
                    }
                }
                _ = sb.Append('}');
                visitor.SetField("xtriggers", sb.ToString().Trim(','));
                // Добавление объекта в список
                newVisitors.Add(visitor);
            }
            visitors.AddRange(newVisitors);
        }

        /// <summary>
        /// Вывод в excel файл
        /// </summary>
        /// <param name="visitors"></param>
        private static void OutputExcelData(List<Visitor> visitors)
        {
            if (visitors.Count == 0)
            {

            }
            Console.Write("Введите название файла без расширения    ");
            string name = Console.ReadLine() ?? "";

            // Определение максимального количества элементов в Aspects, Xexts, AdditionalInfo и AddLabels
            int maxAspects = visitors.Max((Visitor visitor) => visitor.Aspects?.Count ?? 0);
            int maxXexts = visitors.Max((Visitor visitor) => visitor.Xexts?.Count ?? 0);
            int maxAddInfo = visitors.Max((Visitor visitor) => visitor.Xtriggers.AdditionalInfo?.Count ?? 0);
            int maxAddLabels = visitors.Max((Visitor visitor) => visitor.Xtriggers.AdditionalLabels?.Count ?? 0);

            // Создание Excel файла
            using (XLWorkbook workbook = new())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Items");

                // Заголовки
                List<string> headers =
                [
                "Id", "Label", "Desc", "Inherits", "Decayto", "Icon", "Audio", "Comments", "Lifetime"
            ];

                // Добавление заголовков для Aspects
                for (int i = 0; i < maxAspects; i++)
                {
                    headers.Add($"Aspect_Key_{i + 1}");
                    headers.Add($"Aspect_Value_{i + 1}");
                }

                // Добавление заголовков для Xexts
                for (int i = 0; i < maxXexts; i++)
                {
                    headers.Add($"Xext_Key_{i + 1}");
                    headers.Add($"Xext_Value_{i + 1}");
                }

                headers.Add("Satisfying_Info");
                headers.Add("Satisfying_Id");
                headers.Add("Satisfying_Morpheffect");
                headers.Add("Satisfying_Level");
                headers.Add("Appointing_Id");
                headers.Add("Appointing_Morpheffect");
                headers.Add("Appointing_Level");
                headers.Add("Dissatisfying");

                for (int i = 0; i < maxAddInfo; i++)
                {
                    headers.Add($"AdditionalInfo_Key_{i + 1}");
                    headers.Add($"Additional_Id_{i + 1}");
                    headers.Add($"Additional_Morpheffect_{i + 1}");
                    headers.Add($"Additional_Level_{i + 1}");
                }

                for (int i = 0; i < maxAddLabels; i++)
                {
                    headers.Add($"Additional_Key_{i + 1}");
                    headers.Add($"Additional_Label_{i + 1}");
                }


                // Запись заголовков
                for (int i = 0; i < headers.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i];
                }

                // Форматирование заголовков (акцентный цвет)
                IXLRange headerRange = worksheet.Range(1, 1, 1, headers.Count);
                headerRange.Style.Fill.BackgroundColor = XLColor.Yellow;
                headerRange.Style.Font.Bold = true;

                // Запись данных
                for (int i = 0; i < visitors.Count; i++)
                {
                    Visitor visitor = visitors[i];
                    int col = 1;

                    // Основные поля
                    worksheet.Cell(i + 2, col++).Value = visitor.GetField("id");
                    worksheet.Cell(i + 2, col++).Value = visitor.GetField("label");
                    worksheet.Cell(i + 2, col++).Value = visitor.GetField("desc");
                    worksheet.Cell(i + 2, col++).Value = visitor.GetField("inherits");
                    worksheet.Cell(i + 2, col++).Value = visitor.GetField("decayto");
                    worksheet.Cell(i + 2, col++).Value = visitor.GetField("icon");
                    worksheet.Cell(i + 2, col++).Value = visitor.GetField("audio");
                    worksheet.Cell(i + 2, col++).Value = visitor.GetField("comments");
                    worksheet.Cell(i + 2, col++).Value = visitor.GetField("lifetime");

                    // Aspects
                    foreach (KeyValuePair<string, string> aspect in visitor.Aspects)
                    {
                        worksheet.Cell(i + 2, col++).Value = aspect.Key;
                        worksheet.Cell(i + 2, col++).Value = aspect.Value;
                    }

                    // Заполнение пустых ячеек для Aspects (если элементов меньше, чем maxAspects)
                    for (int j = visitor.Aspects.Count; j < maxAspects; j++)
                    {
                        worksheet.Cell(i + 2, col++).Value = "";
                        worksheet.Cell(i + 2, col++).Value = "";
                    }

                    // Xexts
                    foreach (KeyValuePair<string, string> xext in visitor.Xexts)
                    {
                        worksheet.Cell(i + 2, col++).Value = xext.Key;
                        worksheet.Cell(i + 2, col++).Value = xext.Value;
                    }

                    // Заполнение пустых ячеек для Xexts (если элементов меньше, чем maxXexts)
                    for (int j = visitor.Xexts.Count; j < maxXexts; j++)
                    {
                        worksheet.Cell(i + 2, col++).Value = "";
                        worksheet.Cell(i + 2, col++).Value = "";
                    }

                    // Заполнение Xtriggers
                    worksheet.Cell(i + 2, col++).Value = visitor.Xtriggers.SatisfyingLabel;
                    worksheet.Cell(i + 2, col++).Value = visitor.Xtriggers.SatisfyingInfo.Id;
                    worksheet.Cell(i + 2, col++).Value = visitor.Xtriggers.SatisfyingInfo.Morpheffect;
                    worksheet.Cell(i + 2, col++).Value = visitor.Xtriggers.SatisfyingInfo.Level;

                    worksheet.Cell(i + 2, col++).Value = visitor.Xtriggers.AppointingInfo.Id;
                    worksheet.Cell(i + 2, col++).Value = visitor.Xtriggers.AppointingInfo.Morpheffect;
                    worksheet.Cell(i + 2, col++).Value = visitor.Xtriggers.AppointingInfo.Level;

                    worksheet.Cell(i + 2, col++).Value = visitor.Xtriggers.Dissatisfying;

                    //Заполнение AdditionalInfo
                    foreach (KeyValuePair<string, XtriggerInfo> addInfo in visitor.Xtriggers.AdditionalInfo)
                    {
                        worksheet.Cell(i + 2, col++).Value = addInfo.Key;
                        worksheet.Cell(i + 2, col++).Value = addInfo.Value.Id;
                        worksheet.Cell(i + 2, col++).Value = addInfo.Value.Morpheffect;
                        worksheet.Cell(i + 2, col++).Value = addInfo.Value.Level;
                    }

                    // Заполнение пустых ячеек для AdditionalInfo (если элементов меньше, чем maxAddInfo)
                    for (int j = visitor.Xtriggers.AdditionalInfo.Count; j < maxAddInfo; j++)
                    {
                        worksheet.Cell(i + 2, col++).Value = "";
                        worksheet.Cell(i + 2, col++).Value = "";
                        worksheet.Cell(i + 2, col++).Value = "";
                        worksheet.Cell(i + 2, col++).Value = "";
                    }

                    // Заполнение AdditionalLabels
                    foreach (KeyValuePair<string, string> addLabel in visitor.Xtriggers.AdditionalLabels)
                    {
                        worksheet.Cell(i + 2, col++).Value = addLabel.Key;
                        worksheet.Cell(i + 2, col++).Value = addLabel.Value;
                    }

                    // Заполнение пустых ячеек для AdditionalLabels (если элементов меньше, чем maxAddLabels)
                    for (int j = visitor.Xtriggers.AdditionalLabels.Count; j < maxAddLabels; j++)
                    {
                        worksheet.Cell(i + 2, col++).Value = "";
                        worksheet.Cell(i + 2, col++).Value = "";
                    }
                }

                // Автонастройка ширины столбцов
                _ = worksheet.Columns().AdjustToContents();

                workbook.SaveAs($"{UtilsClass.GetDirectory()}{name}.xlsx");
            }

            Console.WriteLine("Excel файл успешно создан.");
        }
    }
}
