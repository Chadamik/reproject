using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JSONLibrary
{
    public interface IJSONObject
    {
        public IEnumerable<string> GetAllFields();
        public string? GetField(string fieldName);
        public void SetField(string fieldName, string value);
    }
}