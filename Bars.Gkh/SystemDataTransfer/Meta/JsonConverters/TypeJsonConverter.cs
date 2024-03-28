namespace Bars.Gkh.SystemDataTransfer.Meta.JsonConverters
{
    using System;
    using System.Linq;

    using Newtonsoft.Json;

    /// <summary>
    /// Конвертер JSON для Type
    /// </summary>
    /// <remarks>
    /// Хранить полный путь типа (стандартный не подходит, т.к. хранит версию сборки и т.п.)
    /// </remarks>
    public class TypeJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((value as Type)?.FullName);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var typeName = reader.Value as string;
           return AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType(typeName)).First(x => x != null);
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return typeof(Type).IsAssignableFrom(objectType);
        }
    }
}