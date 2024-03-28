namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Сущность связи Решения и Нарушения.
    /// </summary>
    public class PrescriptionCancelViolReference : BaseEntity
    {
        /// <summary>
        /// Решение об отмене в предписании ГЖИ
        /// </summary>
        public virtual PrescriptionCancel PrescriptionCancel { get; set; }

        /// <summary>
        /// Этап указания к устранению нарушения в предписании
        /// </summary>
        public virtual InspectionGjiViolStage InspectionViol { get; set; }

        /// <summary>
        /// Новый срок устранения
        /// </summary>
        public virtual DateTime? NewDatePlanRemoval { get; set; }
    }
}
