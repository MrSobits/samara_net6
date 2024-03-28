namespace Bars.Gkh.MetaValueConstructor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.Utils;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Методы-расширения для <see cref='DataMetaInfo'/>
    /// </summary>
    public static class DataMetaInfoExtensions
    {
        /// <summary>
        ///  Увы, другого человеческого способа работы с NCalc с русскими символами я не нашёл
        /// </summary>
        private static readonly IDictionary<char, string> translitDict = new Dictionary<char, string>
        {
            { 'а', "a" },  { 'б', "b" },  { 'в', "v" },  { 'г', "g" },   { 'д', "d" },  { 'е', "e" },   { 'ё', "yo" },
            { 'ж', "zh" }, { 'з', "z" },  { 'и', "i" },  { 'й', "j" },   { 'к', "k" },  { 'л', "l" },   { 'м', "m" },
            { 'н', "n" },  { 'о', "o" },  { 'п', "p" },  { 'р', "r" },   { 'с', "s" },  { 'т', "t" },   { 'у', "u" },
            { 'ф', "f" },  { 'х', "h" },  { 'ц', "c" },  { 'ч', "ch" },  { 'ш', "sh" }, { 'щ', "sch" }, { 'ъ', "j" },
            { 'ы', "i" },  { 'ь', "j" },  { 'э', "e" },  { 'ю', "yu" },  { 'я', "ya" }, { 'А', "A" },   { 'Б', "B" },
            { 'В', "V" },  { 'Г', "G" },  { 'Д', "D" },  { 'Е', "E" },   { 'Ё', "Yo" }, { 'Ж', "Zh" },  { 'З', "Z" },
            { 'И', "I" },  { 'Й', "J" },  { 'К', "K" },  { 'Л', "L" },   { 'М', "M" },  { 'Н', "N" },   { 'О', "O" },
            { 'П', "P" },  { 'Р', "R" },  { 'С', "S" },  { 'Т', "T" },   { 'У', "U" },  { 'Ф', "F" },   { 'Х', "H" },
            { 'Ц', "C" },  { 'Ч', "Ch" }, { 'Ш', "Sh" }, { 'Щ', "Sch" }, { 'Ъ', "J" },  { 'Ы', "I" },   { 'Ь', "J" },
            { 'Э', "E" },  { 'Ю', "Yu" }, { 'Я', "Ya" }
        };

        /// <summary>
        /// Метод транслирует код (т.к. NCalc не понимает русских символов)
        /// </summary>
        /// <param name="metaInfo">Мета-информация</param>
        /// <returns>Пара код - транслит</returns>
        public static KeyValuePair<string, string> GetTranslatedCode(this DataMetaInfo metaInfo)
        {
            return new KeyValuePair<string, string>(metaInfo.Code, metaInfo.Code.Translate());
        }

        /// <summary>
        /// Метод переводит русский текст в транслит
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Выходная строка</returns>
        public static string Translate(this string input)
        {
            var newCode = new StringBuilder();
            input.DecodeFromAnis().ForEach(x => newCode.Append(DataMetaInfoExtensions.translitDict.Get<char, string>(x, x.ToString())));
            return newCode.ToString();
        }

        /// <summary>
        /// Метод возвращает транслированные коды мета-информации
        /// </summary>
        /// <param name="metaInfo">Мета-информация</param>
        /// <returns>Результат</returns>
        public static IDictionary<string, string> GetChildrenCodes(this DataMetaInfo metaInfo)
        {
            return metaInfo.Children.Select(x => x.GetTranslatedCode()).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Декодирует строку из ANSI в UTF-8
        /// </summary>
        /// <param name="ansiString">Строка</param>
        /// <returns>Результат</returns>
        public static string DecodeFromAnis(this string ansiString)
        {
            var byte1251 = Encoding.GetEncoding(1251).GetBytes(ansiString);
            var byteUtf8 = Encoding.Convert(Encoding.GetEncoding(1251), Encoding.UTF8, byte1251);
            var strUtf8 = Encoding.UTF8.GetString(byteUtf8);

            return strUtf8;
        }
    }
}