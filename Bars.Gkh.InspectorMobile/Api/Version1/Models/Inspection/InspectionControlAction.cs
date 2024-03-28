namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Inspection
{
    using System;
    using System.Collections.Generic;

    using Bars.GkhGji.Enums;

    /// <summary>
    /// Сущность описывающая документ КНМ
    /// </summary>
    public class InspectionControlAction
    {
        /// <summary>
        /// Уникальный идентификатор документа
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public TypeDocumentGji DocumentType { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Период обследования с
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Период обследования по
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Уникальный идентификатор контрагента
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// Список домов
        /// </summary>
        public IEnumerable<long> Addresses { get; set; }
    }
}