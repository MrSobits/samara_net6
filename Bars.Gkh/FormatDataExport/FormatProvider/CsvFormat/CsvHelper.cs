namespace Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Класс работы с csv файлами
    /// </summary>
    public static class CsvHelper
    {
        /// <summary>
        /// Получить отфильтрованную строку
        /// </summary>
        public static string GetFiltredString(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }

            var filtredString = data.Replace("\"", "\"\"");

            filtredString = filtredString.Contains(";") ||
                filtredString.Contains("'") ||
                filtredString.Contains("\"") ||
                filtredString.Contains("\n")
                    ? $"\"{filtredString}\""
                    : filtredString;

            return filtredString;
        }

        /// <summary>
        /// Получить csv строку из коллекции
        /// </summary>
        public static string GetRow(IList<string> dataList)
        {
            return string.Join(";", dataList.Select(CsvHelper.GetFiltredString));
        }

        /// <summary>
        /// Получить csv файл
        /// </summary>
        /// <param name="fileNameWithoutExtension">Имя файла без расширения</param>
        /// <param name="header">Заголовок</param>
        /// <param name="data">Данные</param>
        /// <param name="createEmpry">Создать пустой файл при отсутствии данных (Только заголовок)</param>
        public static CsvFileData GetContent(string fileNameWithoutExtension, IList<string> header, IEnumerable<IList<string>> data, bool createEmpry = false)
        {
            var content = new StringBuilder();

            content.AppendLine(CsvHelper.GetRow(header.Select((x, i) => $"{i+1}. {x}").ToList()));
            var headerLength = content.Length;

            foreach (var row in data)
            {
                content.AppendLine(CsvHelper.GetRow(row));
            }

            if (!createEmpry && headerLength == content.Length)
            {
                return new CsvFileData(fileNameWithoutExtension, null);
            }

            return new CsvFileData(fileNameWithoutExtension, content.ToString());
        }
    }
}