namespace Bars.Gkh.Utils.Json
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Конвертер для десериалзиации интерфейсов
    /// </summary>
    /// <typeparam name="TConcrete">Тип интерфейса</typeparam>
    public class ConcreteTypeConverter<TConcrete> : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<TConcrete>(reader);
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}