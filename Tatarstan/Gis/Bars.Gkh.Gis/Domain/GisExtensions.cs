namespace Bars.Gkh.Gis.Domain
{
    using System.Collections.Generic;
    using System.Text;

    public static class GisExtensions
    {
        /// <summary>
        /// Метод объединения строк через разделитель
        /// </summary>
        public static string AggregateWithSeparator(this IEnumerable<string> source, string separator)
        {
            var result = new StringBuilder();

            foreach (var current in source)
            {
                if (!string.IsNullOrWhiteSpace(current))
                {
                    if (result.Length > 0)
                    {
                        result.Append(separator);
                    }

                    result.Append(current);
                }
            }

            return result.ToString();
        }
    }
}