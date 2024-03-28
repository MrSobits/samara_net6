using GisGkhLibrary.Utils.Rfc2047;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GisGkhLibrary.Utils
{
    /// <summary>
    /// Класс шифрования/дешифрования строк
    /// Описывает криво/косо урезанную версию стандарта RFC 2047
    /// </summary>
    public class MimeUtility
    {
        /// <summary>
        /// Regex for parsing encoded word sections
        /// From http://tools.ietf.org/html/rfc2047#section-3
        /// encoded-word = "=?" charset "?" encoding "?" encoded-text "?="
        /// </summary>
        private static Regex EncodedWordFormatRegEx { get; } = new Regex(
            @"=\?(?<charset>.*?)\?(?<encoding>[qQbB])\?(?<encodedtext>.*?)\?=",
            RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// Replacement string for removing CRLF SPACE separators
        /// </summary>
        private static string SeparatorReplacement => @"?==?";

        /// <summary>
        /// Regex for removing CRLF SPACE separators from between encoded words
        /// </summary>
        private static Regex EncodedWordSeparatorRegEx { get; } = new Regex(@"\?=\s+=\?", RegexOptions.Compiled);

        /// <summary>
        /// Шифрует строку по стандарту RFC 2047
        /// </summary>
        /// <param name="value">Исходное значение</param>
        /// <param name="contentEncoding">Алгоритм шифрования</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>Зашифрованное значение</returns>
        public static string Encode(string value, ContentEncoding contentEncoding = ContentEncoding.Base64,
            string encoding = "utf-8")
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            if (!IsSupportedCharacterSet(encoding))
            {
                throw new ArgumentException("Неизвестная кодировка", nameof(encoding));
            }

            if (contentEncoding == ContentEncoding.Unknown)
            {
                throw new ArgumentException("Неизвестный алгоритм шифрования", nameof(contentEncoding));
            }

            IRfc2047Streamer streamer;
            switch (contentEncoding)
            {
                case ContentEncoding.Base64:
                    streamer = new Rfc2047Base64Streamer();
                    break;

                case ContentEncoding.QEncoding:
                    streamer = new Rfc2047QStreamer();
                    break;

                default:
                    throw new NotSupportedException();
            }

            return streamer.Encode(value, encoding);
        }

        /// <summary>
        /// Дешифрует строку по стандарту RFC 2047
        /// </summary>
        /// <param name="value">Зашифрованная строка</param>
        /// <returns>Исходная строка</returns>
        public static string Decode(string value)
        {
            var decodedString = EncodedWordSeparatorRegEx.Replace(value, SeparatorReplacement);
            return EncodedWordFormatRegEx.Replace(
                decodedString,
                m =>
                {
                    IRfc2047Streamer streamer;
                    var contentEncoding = GetContentEncodingType(m.Groups["encoding"].Value);
                    switch (contentEncoding)
                    {
                        case ContentEncoding.Base64:
                            streamer = new Rfc2047Base64Streamer();
                            break;

                        case ContentEncoding.QEncoding:
                            streamer = new Rfc2047QStreamer();
                            break;

                        default: return string.Empty;
                    }

                    if (contentEncoding == ContentEncoding.Unknown)
                    {
                        // Regex should never match, but return anyway
                        return string.Empty;
                    }

                    var characterSet = m.Groups["charset"].Value;
                    if (!IsSupportedCharacterSet(characterSet))
                    {
                        // Fall back to iso-8859-1 if invalid/unsupported character set found
                        characterSet = @"iso-8859-1";
                    }

                    var textEncoding = Encoding.GetEncoding(characterSet);

                    var encodedText = m.Groups["encodedtext"].Value;
                    return streamer.Decode(encodedText, textEncoding);
                });
        }

        /// <summary>
        /// Gets the content encoding type from the encoding character
        /// </summary>
        /// <param name="contentEncodingCharacter">Content contentEncodingCharacter character</param>
        /// <returns>ContentEncoding type</returns>
        private static ContentEncoding GetContentEncodingType(string contentEncodingCharacter)
        {
            switch (contentEncodingCharacter)
            {
                case "Q":
                case "q":
                    return ContentEncoding.QEncoding;
                case "B":
                case "b":
                    return ContentEncoding.Base64;
                default:
                    return ContentEncoding.Unknown;
            }
        }

        /// <summary>
        /// Determines if a character set is supported
        /// </summary>
        /// <param name="characterSet">Character set name</param>
        /// <returns>Bool representing whether the character set is supported</returns>
        private static bool IsSupportedCharacterSet(string characterSet)
        {
            return Encoding.GetEncodings()
                .Any(e => string.Equals(e.Name, characterSet, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}