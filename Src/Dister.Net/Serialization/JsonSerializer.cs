using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Dister.Net.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string s)
            => JsonConvert.DeserializeObject<T>(s);

        public object Deserialize(string s, Type type)
            => JsonConvert.DeserializeObject(s, type);

        public string Serialize(object o)
            => JsonConvert.SerializeObject(o);
    }
}
