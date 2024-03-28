namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    public class PrescriptionCancelTatarstan : PrescriptionCancel
    {
        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата исходящего письма
        /// </summary>
        public virtual DateTime? OutMailDate { get; set; }

        /// <summary>
        /// Номер исходящего письма
        /// </summary>
        public virtual string OutMailNumber { get; set; }

        /// <summary>
        /// Уведомление передано
        /// </summary>
        public virtual YesNo? NotificationTransmission { get; set; }

        /// <summary>
        /// Уведомление получено
        /// </summary>
        public virtual YesNo? NotificationReceive { get; set; }

        /// <summary>
        /// Способ уведомления
        /// </summary>
        public virtual NotificationType? NotificationType { get; set; }

        /// <summary>
        /// Срок продления
        /// </summary>
        public virtual DateTime? ProlongationDate { get; set; }
    }
}