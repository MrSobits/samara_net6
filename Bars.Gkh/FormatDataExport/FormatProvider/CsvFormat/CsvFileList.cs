namespace Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;

    /// <summary>
    /// Список экспортируемых файлов
    /// </summary>
    public class CsvFileList
    {
        private const string FileListName = "_filelist";

        private readonly IList<IList<string>> fileList = new List<IList<string>>();

        private readonly IList<IList<string>> baseFileList = new List<IList<string>>();

        private readonly IList<string> header = new List<string>()
        {
            "Наименование файла",
            "Количество строк данных",
            "Контрольная строка"
        };

        /// <summary>
        /// Добавить информацию о файле
        /// </summary>
        public void Add(CsvFileData fileData)
        {
            this.fileList.Add(new List<string>
            {
                fileData.FullName,
                fileData.RowNumber.ToString(),
                fileData.HashSum
            });

            if (fileData.FileName.ToLower() == "_info" || fileData.FileName.ToLower() == "files")
            {
                this.baseFileList.Add(new List<string>
                {
                    fileData.FullName,
                    fileData.RowNumber.ToString(),
                    fileData.HashSum
                });
            }
        }

        /// <summary>
        /// Получить csv файл
        /// </summary>
        public CsvFileData GetCsvFileData()
        {
            return CsvHelper.GetContent(CsvFileList.FileListName, this.header, this.fileList);
        }

        /// <summary>
        /// Получить csv файл c информацией секций info и files
        /// </summary>
        public CsvFileData GetBaseCsvFileData()
        {
            return CsvHelper.GetContent(CsvFileList.FileListName, this.header, this.baseFileList);
        }
    }
}