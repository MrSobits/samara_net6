namespace Bars.Gkh.Utils
{
    using System.Text;

    /// <summary>
    /// Extention methods for byte[]
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetString(this byte[] array, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetString(array ?? new byte[0]);
        }
    }
}