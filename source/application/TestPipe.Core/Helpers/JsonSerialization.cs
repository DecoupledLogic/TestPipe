namespace TestPipe.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using TestPipe.Core.Exceptions;

    public static class JsonSerialization
    {
        public static object Deserialize(string data, string type)
        {
            return Deserialize(data, GetType(type));
        }

        public static object Deserialize(string data, Type type)
        {
            return JsonConvert.DeserializeObject(data, type);
        }

        public static dynamic Deserialize(TextReader reader)
        {
            var jsonReader = new JsonTextReader(reader);

            try
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize(jsonReader);
            }
            catch (JsonSerializationException e)
            {
                // Wrap in a standard .NET exception.
                throw new SerializationException(e.Message, e);
            }
        }

        public static string Serialize(object value)
        {
            return null == value
                      ? null
                      : JsonConvert.SerializeObject(value);
        }

        public static string GetType(object value)
        {
            return null == value
                      ? null
                      : GetSimpleTypeName(value);
        }

        public static string GetSimpleTypeName(object obj)
        {
            return null == obj
                      ? null
                      : obj.GetType().AssemblyQualifiedName;
        }

        public static Type GetType(string simpleTypeName)
        {
            return Type.GetType(simpleTypeName);
        }
    }
}
