namespace Bars.Esia.OAuth20.App.Extensions
{
    using Newtonsoft.Json.Linq;

    public static class JObjectExtension
    {
        /// <summary>
        /// Получить значение по наименованию
        /// </summary>
        public static string GetPropertyValue(this JObject jObject, string property)
        {
            return jObject.GetValue(property)?.ToString() ?? string.Empty;
        }
    }
}