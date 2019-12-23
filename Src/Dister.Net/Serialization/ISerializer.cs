using System;

namespace Dister.Net.Serialization
{
    /// <summary>
    /// Serializer interface
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes object to string
        /// </summary>
        /// <param name="o">Object to serialize</param>
        /// <returns>Serialized object</returns>
        string Serialize(object o);
        /// <summary>
        /// Deserializes object to T
        /// </summary>
        /// <typeparam name="T">Type of deserilized object</typeparam>
        /// <param name="s">Serialized object</param>
        /// <returns>Deserialized object</returns>
        T Deserialize<T>(string s);
        /// <summary>
        /// Deserializes object to object of specified type
        /// </summary>
        /// <param name="s">Serialized object</param>
        /// <param name="type">Type of deserialized object</param>
        /// <returns>Deserialized object</returns>
        object Deserialize(string s, Type type);
    }
}
