namespace Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat
{
    using System.Text;

    using Bars.B4;

    /// <summary>
    /// Информация о csv файле
    /// </summary>
    public class CsvFileData : FileData
    {
        /// <summary>
        /// Расширение файла
        /// </summary>
        public const string FileExtension = ".csv";

        /// <summary>
        /// Кодировка файла
        /// <para>
        /// Значение по умолчанию: Windows-1251
        /// </para>
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.GetEncoding(1251);

        /// <summary>
        /// Имя файла с расширением
        /// </summary>
        public string FullName => $"{this.FileName}{CsvFileData.FileExtension}";

        /// <summary>
        /// Число строк
        /// </summary>
        public long RowNumber { get; private set; }

        /// <summary>
        /// Контрольная строка
        /// </summary>
        public string HashSum { get; set; }

        /// <inheritdoc />
        protected CsvFileData()
            : base(null, null, null)

        {
        }

        /// <inheritdoc />
        public CsvFileData(string fileName, string content)
            : base(fileName, null, null)

        {
            this.Init(fileName, content, this.Encoding);
        }

        /// <inheritdoc />
        public CsvFileData(string fileName, string content, Encoding encoding)
            : base(fileName, null, null)
        {
            this.Init(fileName, content, encoding);
        }

        private void Init(string fileName, string content, Encoding encoding)
        {
            this.FileName = fileName;
            this.Extention = CsvFileData.FileExtension;
            this.Encoding = encoding;

            if (!string.IsNullOrEmpty(content))
            {
                this.Data = this.Encoding.GetBytes(content);
                this.RowNumber = this.CountLines(content);
            }
        }

        private long CountLines(string s)
        {
            var length = s.Split('\n').LongLength;
            return length == 0 ? 0 : length - 1;
        }

        /// <summary>
        /// Проверка файла на пустоту
        /// </summary>
        /// <remarks>
        /// Файл является пустым, если он не содержит данных или содержит только заголовок
        /// </remarks>
        public bool IsEmpty()
        {
            return this.RowNumber < 2;
        }

        /// <summary>
        /// Создать пустой файл
        /// </summary>
        public static CsvFileData CreateEmpty()
        {
            return new CsvFileData();
        }
    }
}