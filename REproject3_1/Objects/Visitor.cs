using JSONLibrary;
using System.Text;

namespace REproject3_1.Objects
{
    /// <summary>
    /// Структура, представляющая объект-посетитель.
    /// </summary>
    public struct Visitor : IJSONObject
    {
        public string Id { get; private set; }
        public string Label { get; private set; }
        public string Desc { get; private set; }
        public string Inherits { get; private set; }
        public string Decayto { get; private set; }
        public string Icon { get; private set; }
        public string Audio { get; private set; }
        public string Comments { get; private set; }
        public Dictionary<string, string> Aspects { get; private set; }
        public string Lifetime { get; private set; }
        public Dictionary<string, string> Xexts { get; private set; }
        public Xtrigger Xtriggers { get; private set; }

        /// <summary>
        /// Конструктор с пустыми параметрами.
        /// </summary>
        public Visitor()
        {
            Id = "";
            Label = "";
            Desc = "";
            Inherits = "";
            Decayto = "";
            Icon = "";
            Audio = "";
            Comments = "";
            Aspects = [];
            Lifetime = "";
            Xexts = [];
            Xtriggers = new();
        }

        /// <summary>
        /// Конструктор с параметром - строковым представлением объекта Visitor.
        /// </summary>
        /// <param name="json"></param>
        public Visitor(string json) : this()
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
            return ["id", "label", "desc", "icon", "audio", "inherits", "aspects", "decayto", "lifetime", "comments", "xtriggers", "xexts"];
        }

        /// <summary>
        /// Метод, возвращающий значение поля по названию поля.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public readonly string? GetField(string fieldName)
        {
            switch(fieldName)
            {
                case "id":
                    return Id;
                case "label":
                    return Label;
                case "desc": 
                    return Desc;
                case "inherits":
                    return Inherits;
                case "decayto":
                    return Decayto;
                case "icon":
                    return Icon;
                case "audio":
                    return Audio;
                case "comments":
                    return Comments;
                case "lifetime":
                    return Lifetime;
                case "aspects":
                    if (Aspects.Count == 0) { return ""; }
                    StringBuilder sb = new();
                    _ = sb.Append('{');
                    foreach (string key in Aspects.Keys)
                    {
                        _ = sb.Append($"\"{key}\":{Aspects[key]},");
                    }
                    _ = sb.Remove(sb.Length - 1, 1);
                    _ = sb.Append('}');
                    return sb.ToString();
                case "xexts":
                    if (Xexts.Count == 0) { return ""; }
                    StringBuilder stringBuilder = new();
                    _ = stringBuilder.Append('{');
                    foreach (string key in Xexts.Keys)
                    {
                        _ = stringBuilder.Append($"\"{key}\":\"{Xexts[key]}\",");
                    }
                    _ = stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    _ = stringBuilder.Append('}');
                    return stringBuilder.ToString();
                case "xtriggers" :
                    return Xtriggers.ToString() == new Xtrigger().ToString() ? "" : $"{{{Xtriggers}}}";
                default:
                    return null;
            };
        }

        /// <summary>
        /// Метод, присваивающий полю значение по введенному названию поля и значению.
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        /// <param name="value">Значение поля</param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void SetField(string fieldName, string value)
        {
            switch (fieldName)
            {
                case "id":
                    Id = value;
                    break;
                case "label":
                    Label = value;
                    break;
                case "desc":
                    Desc = value; break;
                case "inherits":
                    Inherits = value; break;
                case "decayto":
                    Decayto = value; break;
                case "icon":
                    Icon = value; break;
                case "audio":
                    Audio = value; break;
                case "comments":
                    Comments = value; break;
                case "lifetime":
                    Lifetime = value; break;
                case "xtriggers":
                    Xtriggers = new Xtrigger(value);  break;
                case "aspects":
                    Aspects = JsonParser.ParseJsonToDictionary(value);
                    break;
                case "xexts":
                    Xexts = JsonParser.ParseJsonToDictionary(value);
                    break;
                default:
                    throw new KeyNotFoundException();
            }
        }
        /// <summary>
        /// Представление в виде json.
        /// </summary>
        /// <returns></returns>
        public override readonly string ToString()
        {
            FormatterJsonStrings formatter = new(this);


            string json = "";
            foreach(string fieldName in GetAllFields())
            {
                if (fieldName == "lifetime")
                {
                    json += formatter.GetIntToJson(fieldName); // рассматриваем отдельным случаем, так как lifetime - число
                }
                else
                {
                    json += formatter.GetStringFieldToJson(fieldName);
                }
            }
            return formatter.RemoveExtraComma(json);

        }
        /// <summary>
        /// Представление объекта типа Visitor в виде строки (для основной задачи).
        /// </summary>
        /// <returns></returns>
        public string GetStringForMainTask()
        {
            //представляем аспекты в виде строки(не json)
            if (Aspects.Count == 0) { return "\tНет аспектов"; }
            StringBuilder sb = new();
            foreach (string key in Aspects.Keys)
            {
                _ = sb.Append($"\t{key}: {Aspects[key]}\n");
            }
            _ = sb.Remove(sb.Length - 1, 1);

            string aspectsString = sb.ToString();

            return $"Персонаж:{Label}\n\nОписание:{Desc}\n\nАспекты:\n{aspectsString}";
        }
    }
}