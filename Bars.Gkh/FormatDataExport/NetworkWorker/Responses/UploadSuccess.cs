namespace Bars.Gkh.FormatDataExport.NetworkWorker.Responses
{
    using System;

    internal class UploadSuccess
    {
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Оригинальное имя файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Время создания файла
        /// </summary>
        public DateTimeOffset LastAccess { get; set; }

        /// <summary>
        /// Присвоенное имя файла
        /// </summary>
        public string CachedName { get; set; }

        /// <summary>
        /// Размер файла
        /// </summary>
        public long Size { get; set; }
    }
}