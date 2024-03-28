namespace Bars.Gkh.Config.Impl.Internal.Serialization
{
    using System;
    using System.IO;
    using System.Linq;

    using Bars.B4.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal static class ConfigSerializer
    {
        static ConfigSerializer()
        {
            ConfigSerializer.Serializer = new JsonSerializer();
            // Serializer.Converters.Add(new StringEnumConverter());
            ConfigSerializer.Serializer.Converters.Add(new DynamicDictionaryJsonConverter());
            ConfigSerializer.Serializer.ContractResolver = new ConfigContractResolver();
            ConfigSerializer.Serializer.Formatting = Formatting.Indented;
        }

        public static JsonSerializer Serializer { get; private set; }

        public static string Serialize(object @object)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    ConfigSerializer.Serializer.Serialize(jsonWriter, @object);
                    return stringWriter.ToString();
                }
            }
        }

        public static string Serialize(JObject @object)
        {
            return @object.ToString(Formatting.Indented, ConfigSerializer.Serializer.Converters.ToArray());
        }

        public static T Deserialize<T>(string json)
        {
            return (T) ConfigSerializer.Deserialize(json, typeof (T));
        }

        public static object Deserialize(string json, Type type)
        {
            using (var stringReader = new StringReader(json))
            {
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    return ConfigSerializer.Serializer.Deserialize(jsonReader, type);
                }
            }
        }
    }
}