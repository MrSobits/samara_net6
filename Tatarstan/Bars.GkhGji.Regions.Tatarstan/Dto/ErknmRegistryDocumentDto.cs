namespace Bars.GkhGji.Regions.Tatarstan.Dto
{
    using System;

    using Bars.GkhGji.Enums;

    /// <summary>
    /// DTO'шка реестра "Интеграция с ЕРКНМ"
    /// </summary>
    public class ErknmRegistryDocumentDto
    {
        /// <summary>
        /// Идентификатор
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

        /// <summary>
        /// Идентификатор ЕРКНМ
        /// </summary>
        public string ErknmGuid { get; set; }

        /// <summary>
        /// Дата присвоения учетного номера / идентификатора ЕРКНМ
        /// </summary>
        public DateTime? ErknmRegistrationDate { get; set; }

        /// <summary>
        /// Учетный номер в ЕРКНМ
        /// </summary>
        public string ErknmRegistrationNumber { get; set; }

        /// <summary>
        /// Идентификатор основания проверки
        /// </summary>
        public long InspectionId { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public TypeDocumentGji DocumentType { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public TypeBase DocumentTypeBase { get; set; }

        /// <summary>
        /// Время последнего запуска отправки
        /// </summary>
        public DateTime LastMethodStartTime { get; set; }
    }
}