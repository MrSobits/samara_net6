namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Common
{
    using System;

    /// <summary>
    /// Базовый класс документа
    /// </summary>
    public class BaseDocument
    {
        /// <summary>
        /// Уникальный идентификатор документа
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime? DocumentDate { get; set; }
    }
}