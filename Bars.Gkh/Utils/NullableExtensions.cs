namespace Bars.Gkh.Utils
{
    using System;
    using System.Globalization;

    public static class NullableExtensions
    {
        private static readonly CultureInfo RuCulture = new CultureInfo("ru-RU");

        /// <summary>
        /// В случае наличия значения не равного DateTime.MinValue, вернет строку даты в указанном формате (по умолчанию "d" - эквивалентно ToShortDateString),
        /// иначе вернет пустую строку
        /// </summary>
        /// <param name="date">Экземпляр DateTime?</param>
        /// <param name="format">Формат</param>
        /// <returns>Строка даты в указанном формате</returns>
        public static string ToDateString(this DateTime? date, string format = "d")
        {
            return date.HasValue && date.Value != DateTime.MinValue
                ? date.Value.ToString(format, RuCulture)
                : "";
        }

        /// <summary>
        /// В случае наличия значения не равного DateTime.MinValue, вернет строку времени в указанном формате (по умолчанию "t" - эквивалентно ToShortTimeString),
        /// иначе вернет пустую строку
        /// </summary>
        /// <param name="date">Экземпляр DateTime?</param>
        /// <param name="format">Формат</param>
        /// <returns>Строка времени в указанном формате</returns>
        public static string ToTimeString(this DateTime? date, string format = "t")
        {
            return date.HasValue && date.Value != DateTime.MinValue
                ? date.Value.ToString(format, RuCulture)
                : "";
        }

        /// <summary>
        /// Приводит <see cref="ValueType"/> к <see cref="Nullable"/>
        /// </summary>
        public static T? ToNullable<T>(this T val)
            where T : struct
        {
            return val;
        }
    }
}