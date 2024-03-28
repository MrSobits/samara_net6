namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr
{
    using System;

    /// <summary>
    /// Протоколы и акты
    /// </summary>
    public class Protocol
    {
        /// <summary>
        /// Тип документа
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Участник
        /// </summary>
        public string Contragent { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        public long? FileId { get; set; }
    }
}