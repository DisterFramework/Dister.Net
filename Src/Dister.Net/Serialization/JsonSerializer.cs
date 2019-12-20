using System;
using Dister.Net.Exceptions.SerializationExceptions;
using Newtonsoft.Json;

namespace Dister.Net.Serialization
{
    /// <summary>
    /// Serializer that uses <see cref="Newtonsoft.Json "/> library to serialize and deserilize objects
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception ex)
            {
                throw new DeserializationException(ex);
            }
        }
        public object Deserialize(string s, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(s, type);
            }
            catch (Exception ex)
            {
                throw new DeserializationException(ex);
            }
        }
        public string Serialize(object o)
        {
            try
            {
                return JsonConvert.SerializeObject(o);
            }
            catch (Exception ex)
            {
                throw new SerializationException(ex);
            }
        }
    }
}
