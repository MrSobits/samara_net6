namespace Bars.Gkh.DocIoGenerator
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    public interface IDocIo
    {
        /// <summary>Открыть шаблон</summary>
        /// <param name="stream">Шаблон</param>
        void OpenTemplate(Stream stream);

        /// <summary>Открыть шаблон</summary>
        /// <param name="binaryData">Бинарный шаблон</param>
        void OpenTemplate(byte[] binaryData);

        /// <summary>Закрыть шаблон</summary>
        void CloseTemplate();

        /// <summary>Установить значение по ключу</summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        void SetValue(string key, object value);

        void SetPicture(string key, Stream stream);

        void SetPicture(string key, Stream stream, int width, int height);
        
        bool TrySetPicture(string key, Stream stream, int width, int height);

        void AddTable(DataTable dataTable);

        void AddTable(DataTable dataTable, string name);

        void AddTable(DataTable dataTable, string name, List<int?> columnWidth);

        /// <summary>Получить документ</summary>
        void SaveDocument(Stream document);
     
        /// <summary>
        /// Конвертировать поток файла в PDF формат
        /// </summary>
        Stream ConvertToPdf(Stream stream, string newFileName = null);

        /// <summary>
        /// Конвертировать поток файла в DOC формат
        /// </summary>
        Stream ConvertToDoc(Stream stream, string newFileName = null);
    }
}