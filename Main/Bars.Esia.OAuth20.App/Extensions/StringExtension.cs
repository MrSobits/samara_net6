namespace Bars.Esia.OAuth20.App.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4.Utils;

    public static class StringExtension
    {
        /// <summary>
        /// Нормализировать Uri
        /// </summary>
        public static string NormalizeUri(params string[] urlParts)
        {
            var stringBuilder = new StringBuilder();

            foreach (var urlPart in urlParts)
            {
                stringBuilder.Append(urlPart);
                if (urlPart.Last() != '/' && urlPart.Last() != '\\')
                    stringBuilder.Append("/");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Получить DateTime из Unix Time Stamp'а (по количеству секунд)
        /// </summary>
        public static DateTime? DateFromUnixSeconds(this string seconds)
        {
            DateTime? result = null;

            if (seconds.IsNotEmpty() && double.TryParse(seconds, NumberStyles.Integer, CultureInfo.InvariantCulture, out var doubleParseResult))
            {
                result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(doubleParseResult)
                    .ToLocalTime();
            }

            return result.HasValue && result.Value != DateTime.MinValue ? result : null;
        }
    }
}