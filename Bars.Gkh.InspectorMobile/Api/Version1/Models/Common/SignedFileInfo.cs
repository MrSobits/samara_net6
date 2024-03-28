namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Common
{
    using System;

    /// <summary>
    /// Информация о подписанном файле документа
    /// </summary>
    public class SignedFileInfo
    {
        /// <summary>
        /// Уникальный идентификатор записи 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор подписанного файла
        /// </summary>
        public long? FileId { get; set; }

        /// <summary>
        /// Наименование подписанного файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Дата создания подписанного файла
        /// </summary>
        public DateTime FileDate { get; set; }
    }
}