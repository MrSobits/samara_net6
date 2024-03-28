using System;
using System.Text;

namespace GisGkhLibrary.Utils.Rfc2047
{
    /// <summary>
    /// Шифратор/дешифратор B-фразы по стандарту RFC2047
    /// </summary>
    public class Rfc2047Base64Streamer : Rfc2047Streamer
    {
        /// <summary>
        /// Служебный символ кодировки
        /// </summary>
        protected override string ContectEncodingChar => "B";

        /// <summary>
        /// Создает блок
        /// </summary>
        /// <param name="buffer">Буффер</param>
        /// <param name="bufferLenght">Размер данных</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns></returns>
        protected override string CreateBlock(byte[] buffer, int bufferLenght, Encoding encoding)
        {
            return Convert.ToBase64String(buffer, 0, bufferLenght);
        }

        /// <summary>
        /// Расшифровать фразу
        /// </summary>
        /// <param name="value">Зашифрованная фраза</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>Исходное значение</returns>
        public override string Decode(string value, Encoding encoding)
        {
            return encoding.GetString(Convert.FromBase64String(value));
        }
    }
}