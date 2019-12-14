using System;

namespace Dister.Net.Serialization
{
    public interface ISerializer
    {
        string Serialize(object o);
        T Deserialize<T>(string s);
        object Deserialize(string s, Type type);
    }
}
