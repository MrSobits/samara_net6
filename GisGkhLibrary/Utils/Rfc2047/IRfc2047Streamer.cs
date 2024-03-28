using System.Text;

namespace GisGkhLibrary.Utils.Rfc2047
{
    /// <summary>
    /// Represents a content encoding type defined in RFC2047
    /// </summary>
    public enum ContentEncoding
    {
        /// <summary>
        /// Unknown / invalid encoding
        /// </summary>
        Unknown,

        /// <summary>
        /// "Q Encoding" (reduced character set) encoding
        /// http://tools.ietf.org/html/rfc2047#section-4.2
        /// </summary>
        QEncoding,

        /// <summary>
        /// Base 64 encoding
        /// http://tools.ietf.org/html/rfc2047#section-4.1
        /// </summary>
        Base64
    }

    /// <summary>
    /// Шифратор/дешифратор фразы по стандарту RFC2047
    /// </summary>
    public interface IRfc2047Streamer
    {
        /// <summary>
        /// Зашифроваить фразу
        /// </summary>
        /// <param name="value">Исходное значение</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>Зашифрованное значение</returns>
        string Encode(string value, string encoding);

        /// <summary>
        /// Расшифровать фразу
        /// </summary>
        /// <param name="value">Зашифрованная фраза</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>Исходное значение</returns>
        string Decode(string value, Encoding encoding);
    }
}