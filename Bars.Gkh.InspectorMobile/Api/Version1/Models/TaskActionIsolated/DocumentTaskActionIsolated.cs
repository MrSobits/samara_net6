namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskActionIsolated
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Документ "Задание"
    /// </summary>
    public class DocumentTaskActionIsolated : BaseDocument
    {
        /// <summary>
        /// Проверка
        /// </summary>
        public InspectionInfo Inspection { get; set; }

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string Individual { get; set; }

        /// <summary>
        /// Вид мероприятия
        /// </summary>
        public KindAction TypeCheckId { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public long TypeControlId { get; set; }

        /// <summary>
        /// Основание мероприятия
        /// </summary>
        public TypeBaseAction Base { get; set; }

        /// <summary>
        /// Обращение гражданина
        /// </summary>
        public string Appeal { get; set; }

        /// <summary>
        /// План
        /// </summary>
        public string Plan { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime? EventDate { get; set; }

        /// <summary>
        /// Время
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        /// Список идентификаторов домов
        /// </summary>
        public IEnumerable<long> Addresses { get; set; }

        /// <summary>
        /// ДЛ, вынесшее задание
        /// </summary>
        public long? ExecutiveId { get; set; }

        /// <summary>
        /// Список идентификаторов инспекторов
        /// </summary>
        public IEnumerable<long> InspectorIds { get; set; }

        /// <summary>
        /// Является конечным
        /// </summary>
        public bool RelatedDocuments { get; set; }
    }
}