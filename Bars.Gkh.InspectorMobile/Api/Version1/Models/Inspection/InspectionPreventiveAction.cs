using Bars.GkhGji.Regions.Tatarstan.Enums;
using System;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Inspection
{
    /// <summary>
    /// Документ "Задание по профилактическим мероприятиям"
    /// </summary>
    public class InspectionPreventiveActionTask
    {
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Вид мероприятия
        /// </summary>
        public PreventiveActionType TypeCheckId { get; set; }

        /// <summary>
        /// Тип визита
        /// </summary>
        public PreventiveActionVisitType? TypeVisit { get; set; }

        /// <summary>
        /// Номер документа "Задание"
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа "Задание"
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата мероприятия
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Идентификатор организации
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// Адрес дома - места проведения мероприятия
        /// </summary>
        public string Address { get; set; }
    }
}