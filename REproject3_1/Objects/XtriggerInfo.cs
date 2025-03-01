using JSONLibrary;

namespace REproject3_1.Objects
{
    /// <summary>
    /// Структура, представляющая объект с полями "id", "morpheffect", "level" в 
    /// </summary>
    public struct XtriggerInfo : IJSONObject
    {
        public string Id { get; private set; }
        public string Morpheffect { get; private set; }
        public string Level { get; private set; }

        /// <summary>
        /// Конструктор с пустыми параметрами.
        /// </summary>
        public XtriggerInfo()
        {
            Id = "";
            Morpheffect = "";
            Level = "";
        }

        /// <summary>
        /// Конструктор с заданными параметрами.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="morpheffect"></param>
        /// <param name="level"></param>
        public XtriggerInfo(string json) : this()
        {
            Dictionary<string, string> fields = JsonParser.ParseJsonToDictionary(json);

            foreach (string key in fields.Keys)
            {
                SetField(key, fields[key]);
            }
        }
        /// <summary>
        /// Список всех полей объекта.
        /// </summary>
        /// <returns></returns>
        public readonly IEnumerable<string> GetAllFields()
        {
            return ["id", "morpheffect", "level"];
        }
        /// <summary>
        /// Метод, возвращающий значение поля по названию поля.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public readonly string? GetField(string fieldName)
        {
            return fieldName switch
            {
                "id" => Id,
                "morpheffect" => Morpheffect,
                "level" => Level,
                _ => null
            };

        }
        /// <summary>
        /// Метод, присваивающий полю значение по введенному названию поля и значению.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void SetField(string fieldName, string value)
        {
            switch (fieldName)
            {
                case "id":
                    Id = value;
                    break;
                case "morpheffect":
                    Morpheffect = value; break;
                case "level":
                    Level = value;
                    break;
                default:
                    throw new KeyNotFoundException($"{fieldName}");
            }
        }
        /// <summary>
        /// Представление объекта в формате json.
        /// </summary>
        /// <returns></returns>
        public override readonly string ToString()
        {
            FormatterJsonStrings formatter = new(this);

            string json = "";

            foreach (string fieldName in GetAllFields())
            {
                if (fieldName == "level")
                {
                    json += formatter.GetIntToJson(fieldName);
                }
                else
                {
                    json += formatter.GetStringFieldToJson(fieldName);
                }
            }

            return formatter.RemoveExtraComma(json);
        }


    }
}

