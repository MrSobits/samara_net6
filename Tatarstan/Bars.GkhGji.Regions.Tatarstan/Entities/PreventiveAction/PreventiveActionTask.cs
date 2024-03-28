namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using System;

    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Задание профилактического мероприятия
    /// </summary>
    public class PreventiveActionTask : DocumentGji
    {
        /// <summary>
        /// Вид мероприятия
        /// </summary>
        public virtual PreventiveActionType ActionType { get; set; }
        
        /// <summary>
        /// Тип визита
        /// </summary>
        public virtual PreventiveActionVisitType? VisitType { get; set; }
        
        /// <summary>
        /// Способ консультирования
        /// </summary>
        public virtual PreventiveActionCounselingType? CounselingType { get; set; }
        
        /// <summary>
        /// Место проведения мероприятия
        /// </summary>
        public virtual FiasAddress ActionLocation { get; set; }

        /// <summary>
        /// Дата начала проведения мероприятия
        /// </summary>
        public virtual DateTime? ActionStartDate { get; set; }
        
        /// <summary>
        /// Дата окончания проведения мероприятия
        /// </summary>
        public virtual DateTime? ActionEndDate { get; set; }

        /// <summary>
        /// Время начала мероприятия
        /// </summary>
        public virtual DateTime? ActionStartTime { get; set; }
        
        /// <summary>
        /// ДЛ, вынесшее задание
        /// </summary>
        public virtual Inspector TaskingInspector { get; set; }
        
        /// <summary>
        /// Ответственный за исполнение
        /// </summary>
        public virtual Inspector Executor { get; set; }

        /// <summary>
        /// Структурное подразделение
        /// </summary>
        public virtual string StructuralSubdivision { get; set; }

        /// <summary>
        /// Дата уведомления
        /// </summary>
        public virtual DateTime? NotificationDate { get; set; }

        /// <summary>
        /// Уведомление. Дата исходящего письма
        /// </summary>
        public virtual DateTime? OutgoingLetterDate { get; set; }

        /// <summary>
        /// Уведомление передано
        /// </summary>
        public virtual YesNo? NotificationSent { get; set; }

        /// <summary>
        /// Способ уведомления
        /// </summary>
        public virtual NotificationType? NotificationType { get; set; }

        /// <summary>
        /// Уведомление. Номер документа
        /// </summary>
        public virtual string NotificationDocumentNumber { get; set; }
        
        /// <summary>
        /// Уведомление. Номер исходящего письма
        /// </summary>
        public virtual string OutgoingLetterNumber { get; set; }
        
        /// <summary>
        /// Уведомление получено
        /// </summary>
        public virtual YesNo? NotificationReceived { get; set; }
        
        /// <summary>
        /// Отказ от участия в профилактическом мероприятии
        /// </summary>
        public virtual YesNo ParticipationRejection { get; set; }
    }
}