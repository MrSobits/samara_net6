using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GisGkhLibrary.Utils.Rfc2047
{
    /// <summary>
    /// Шифратор/дешифратор Q-фразы по стандарту RFC2047
    /// </summary>
    public class Rfc2047QStreamer : Rfc2047Streamer
    {
        /// <summary>
        /// Специальные символы, определенные в офрмате RFC2047
        /// </summary>
        private static readonly char[] SpecialCharacters =
        {
            '(', ')', '<', '>', '@', ',',
            ';', ':', '<', '>', '/', '[',
            ']', '?', '.', '=', '\t'
        };

        /// <summary>
        /// Служебный символ кодировки
        /// </summary>
        protected override string ContectEncodingChar => "Q";

        /// <summary>
        /// Создает блок
        /// </summary>
        /// <param name="buffer">Буффер</param>
        /// <param name="bufferLenght">Размер данных</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns></returns>
        protected override string CreateBlock(byte[] buffer, int bufferLenght, Encoding encoding)
        {
            var specialBytes = encoding.GetBytes(SpecialCharacters);
            var builder = new StringBuilder(bufferLenght);

            for (var i = 0; i < bufferLenght; i++)
            {
                var charSize = 1;
                if ((buffer[i] & 0xF0) == 0xF0) charSize = 4;
                if ((buffer[i] & 0xE0) == 0xE0) charSize = 3;
                if ((buffer[i] & 0xC0) == 0xC0) charSize = 2;
                if (buffer[i] < 0x80 && !specialBytes.Contains(buffer[i]) && charSize == 1)
                {
                    builder.Append(Convert.ToChar(buffer[i]));
                    continue;
                }

                var j = 0;
                for (; j < charSize; j++)
                {
                    if (j != 0 && (buffer[i + j] & 0x80) != 0x80)
                    {
                        throw new InvalidCastException();
                    }

                    builder.Append("=");
                    builder.Append(Convert.ToString(buffer[i + j], 16).ToUpper());
                }

                i += j - 1;
            }

            return builder.Replace(' ', '_').ToString();
        }

        /// <summary>
        /// Расшифровать фразу
        /// </summary>
        /// <param name="value">Зашифрованная фраза</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>Исходное значение</returns>
        public override string Decode(string value, Encoding encoding)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != '=')
                {
                    builder.Append(value[i]);
                    continue;
                }

                var charBytes = GetCharacterBytes(value, i);
                builder.Append(encoding.GetChars(charBytes));
                i += charBytes.Length * 3 - 1;
            }

            return builder.Replace('_', ' ').ToString();
        }

        /// <summary>
        /// Получить набор байт символа (первые N бит до нуля равно количеству байт в символе)
        /// </summary>
        /// <param name="value">Зашифрованная строка</param>
        /// <param name="offset">Смещение</param>
        /// <returns>Набор байт следующего символа</returns>
        private byte[] GetCharacterBytes(string value, int offset)
        {
            if (!int.TryParse(value.Substring(offset + 1, 2), NumberStyles.HexNumber,
                CultureInfo.InvariantCulture, out var characterByte))
            {
                throw new InvalidOperationException();
            }

            var charSize = 1;
            if ((characterByte & 0xF0) == 0xF0) charSize = 4;
            if ((characterByte & 0xE0) == 0xE0) charSize = 3;
            if ((characterByte & 0xC0) == 0xC0) charSize = 2;

            var bytes = new List<byte> { (byte)characterByte };
            for (var i = 1; i < charSize; i++)
            {
                if (!int.TryParse(value.Substring(offset + i * 3 + 1, 2), NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out characterByte))
                {
                    throw new InvalidOperationException();
                }

                if ((characterByte & 0x80) != 0x80)
                {
                    throw new InvalidCastException();
                }

                bytes.Add((byte)characterByte);
            }

            return bytes.ToArray();
        }
    }
}