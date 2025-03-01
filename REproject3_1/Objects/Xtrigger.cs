using JSONLibrary;

namespace REproject3_1.Objects
{
    /// <summary>
    /// Структура, представляющая объект Xtrigger в объекте Visitor.
    /// </summary>
    public struct Xtrigger : IJSONObject
    {
        public XtriggerInfo SatisfyingInfo { get; private set; }
        public XtriggerInfo AppointingInfo { get; private set; }
        public Dictionary<string, XtriggerInfo> AdditionalInfo { get; private set; }
        public Dictionary<string, string> AdditionalLabels { get; private set; }
        public string SatisfyingLabel { get; private set; }
        public string Dissatisfying { get; private set; }

        /// <summary>
        /// Конструктор с пустыми параметрами.
        /// </summary>
        public Xtrigger()
        {
            AdditionalInfo = [];
            AdditionalLabels = [];
            SatisfyingInfo = new();
            AppointingInfo = new();
            SatisfyingLabel = "";
            Dissatisfying = "";
        }

        /// <summary>
        /// Конструктор с заданными параметрами.
        /// </summary>
        /// <param name="additionalInfo"></param>
        /// <param name="additionalLabels"></param>
        /// <param name="satisfyingLabel"></param>
        /// <param name="appointingInfo"></param>
        /// <param name="satisfyingInfo"></param>
        /// <param name="dissatisfying"></param>
        public Xtrigger(string json) : this()
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
            List<string> fields = ["satisfying", "appointing", "dissatisfying"];
            fields.AddRange(AdditionalInfo.Keys);
            fields.AddRange(AdditionalLabels.Keys);
            return fields;
        }
        /// <summary>
        /// Метод, возвращающий значение поля по названию поля.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public readonly string? GetField(string fieldName)
        {
            switch (fieldName)
            {
                case "satisfying":
                    return SatisfyingLabel != ""
                ? SatisfyingInfo.ToString() == new XtriggerInfo().ToString()
                    ? SatisfyingLabel
                    : $"[\"{SatisfyingLabel}\",{{{SatisfyingInfo}}}]"
                : "";
                case "appointing":
                    return AppointingInfo.ToString() != "" ?  $"[{{{AppointingInfo}}}]" : "";
                case "dissatisfying":
                    return Dissatisfying;
                default:
                    if (AdditionalInfo.TryGetValue(fieldName, out XtriggerInfo value)) { return value.ToString() != "" ?  $"[{{{value}}}]" : ""; }
                    if (AdditionalLabels.TryGetValue(fieldName, out string? val)) { return val; }
                    return null;

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
                case "satisfying":
                    if (value.StartsWith('['))
                    {
                        List<string> satisfyings = JsonParser.SplitArray(value);
                        SatisfyingLabel = satisfyings[0].Trim('"');
                        if (satisfyings.Count == 2)
                        {
                            SatisfyingInfo = new(satisfyings[1]);
                        }
                    }
                    else
                    {
                        SatisfyingLabel = value;
                    }
                    break;
                case "appointing":
                    AppointingInfo = new(value.Trim('[', ']'));
                    break;
                case "dissatisfying":
                    Dissatisfying = value;
                    break;
                default:
                    if (value[0] == '[' && value[^1] == ']')
                    {
                        AdditionalInfo.Add(fieldName, new(value.Trim('[', ']')));
                    }
                    else
                    {
                        AdditionalLabels.Add(fieldName, value);
                    }
                    break;
            }
        }

        /// <summary>
        /// Представление в формате json.
        /// </summary>
        /// <returns></returns>
        public override readonly string ToString()
        {
            FormatterJsonStrings formatter = new(this);

            string json = "";
            foreach (string fieldName in GetAllFields())
            {
                json += formatter.GetStringFieldToJson(fieldName);
            }
            return formatter.RemoveExtraComma(json);
        }
    }
}