namespace Bars.Gkh.Gis.Utils
{
    using System;
    using B4.Utils;
    using Newtonsoft.Json;

    /// <summary>
    /// Конвертер timestamp даты в DateTime
    /// </summary>
    public class TimestampDateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = long.Parse(reader.Value.ToStr());
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(t).ToLocalTime();
            return dtDateTime;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var unixTimestamp = ((DateTime)value).Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            writer.WriteValue(unixTimestamp);
        }
    }
}