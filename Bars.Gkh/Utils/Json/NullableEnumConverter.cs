namespace Bars.Gkh.Utils.Json
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Конвертер JSON <see cref="Nullable{Enum}"/>
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class NullableEnumConverter<TEnum> : JsonConverter
        where TEnum : struct
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            TEnum? result = null;
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var value = serializer.Deserialize<int>(reader);
            if (Enum.IsDefined(typeof(TEnum), value))
            {
                result = (TEnum)Enum.ToObject(typeof(TEnum), value);
            }

            return result;
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, (int?)value);
        }
    }
}