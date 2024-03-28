using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GisGkhLibrary.Utils.Rfc2047
{
    /// <summary>
    /// Шифратор/дешифратор фразы по стандарту RFC2047
    /// </summary>
    public abstract class Rfc2047Streamer : IRfc2047Streamer
    {
        /// <summary>
        /// Информация о символе
        /// </summary>
        protected class CharInfo
        {
            /// <summary>
            /// Код символа
            /// </summary>
            public byte[] Code { get; set; }

            /// <summary>
            /// Индекс симввола
            /// </summary>
            public int Index { get; set; }
        }

        /// <summary>
        /// Максимальная длина строки (включая заголовок)
        /// </summary>
        protected const int MaxLineLength = 75;

        /// <summary>
        /// Шаблон заголовка строки
        /// </summary>
        protected const string HeaderTemplate = "=?{0}?{1}?{2}?=";

        /// <summary>
        /// Сепаратор слов (в RFC 2047 описан как CRLF Space, нам такое не подходит...)
        /// </summary>
        protected const string Separator = " ";

        /// <summary>
        /// Служебный символ кодировки
        /// </summary>
        protected abstract string ContectEncodingChar { get; }

        /// <summary>
        /// Зашифроваить фразу
        /// </summary>
        /// <param name="value">Исходное значение</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>Зашифрованное значение</returns>
        public virtual string Encode(string value, string encoding)
        {
            var encoder = Encoding.GetEncoding(encoding);
            var bytes = value.Select((c, i) => new CharInfo { Code = encoder.GetBytes(new[] { c }), Index = i }).ToList();

            var firstBlock = true;
            var builder = new StringBuilder();
            foreach (var block in CreateBlocks(bytes.ToArray(), encoding))
            {
                if (!firstBlock) builder.Append(Separator);
                firstBlock = false;
                builder.Append(block);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Создать блоки для не Ascii сивволов
        /// </summary>
        /// <param name="characters">Метаданные символов</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns></returns>
        protected virtual IEnumerable<string> CreateBlocks(IReadOnlyList<CharInfo> characters, string encoding)
        {
            var blocksLength = characters.Sum(x => x.Code.Length);
            var headerLength = string.Format(HeaderTemplate, encoding, ContectEncodingChar, string.Empty).Length;
            var maxLineLength = MaxLineLength - headerLength;
            var offset = 0;
            int bufferLenght;
            for (var i = 0; i < blocksLength; i += bufferLenght)
            {
                var buffer = new byte[blocksLength - i > maxLineLength ? maxLineLength : blocksLength - i];
                bufferLenght = buffer.Length;
                for (var b = 0; b < buffer.Length; b += characters[offset - 1].Code.Length)
                {
                    if (b + characters[offset].Code.Length > buffer.Length)
                    {
                        bufferLenght = b;
                        break;
                    }

                    for (var charByteIndex = 0; charByteIndex < characters[offset].Code.Length; charByteIndex++)
                    {
                        buffer[b + charByteIndex] = characters[offset].Code[charByteIndex];
                    }

                    offset++;
                }

                yield return string.Format(HeaderTemplate, encoding, ContectEncodingChar,
                    CreateBlock(buffer, bufferLenght, Encoding.GetEncoding(encoding)));
            }
        }

        /// <summary>
        /// Создает блок
        /// </summary>
        /// <param name="buffer">Буффер</param>
        /// <param name="bufferLenght">Размер данных</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns></returns>
        protected abstract string CreateBlock(byte[] buffer, int bufferLenght, Encoding encoding);

        /// <summary>
        /// Расшифровать фразу
        /// </summary>
        /// <param name="value">Зашифрованная фраза</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>Исходное значение</returns>
        public abstract string Decode(string value, Encoding encoding);
    }
}