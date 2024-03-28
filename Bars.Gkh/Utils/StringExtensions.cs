namespace Bars.Gkh.Utils
{
    using System.Text;
    using B4.Utils;

    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Cut(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length < length)
            {
                return value;
            }

            return value.Substring(0, length);
        }

        /// <summary>
        /// Преобразовать строку в массив байт с указанной кодировкой (по дефолту UTF8)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string value, Encoding encoding = null)
        {
            if(value.IsEmpty()) return new byte[0];

            return (encoding ?? Encoding.UTF8).GetBytes(value);
        }
        /// <summary>
        /// Оборачивает строку в кавычки
        /// </summary>
        /// <param name="str"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        public static string ToStringWithQuote(this string str, string quote = "'")
        {
            if (str == null)
            {
                str = "";
            }
            return string.Format("{0}{1}{0}", quote, str);
        }
    }
}