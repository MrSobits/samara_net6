namespace Bars.GisIntegration.Base.Entities.Inspection
{
    using System;
    using System.Collections.Generic;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Протокол
    /// </summary>
    public class Offence : BaseRisEntity
    {
        /// <summary>
        /// Родительская проверка
        /// </summary>
        public virtual Examination Examination { get; set; }

        /// <summary>
        /// Номер протокола
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата составления протокола
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Вложения. Нехранимое поле. Найти вложения можно через сущность OffenceAttachment
        /// </summary>
        public virtual List<Attachment> Attachments { get; set; }

        /// <summary>
        /// Отменен ли протокол
        /// </summary>
        public virtual bool IsCancelled { get; set; }

        /// <summary>
        /// Причина отмены документа
        /// </summary>
        public virtual string CancelReason { get; set; }

        /// <summary>
        /// Дата отмены документа
        /// </summary>
        public virtual DateTime? CancelDate { get; set; }

        /// <summary>
        /// Номер решения об отмене
        /// </summary>
        public virtual string CancelDecisionNumber { get; set; }

        /// <summary>
        /// Сведения об исполнении
        /// </summary>
        public virtual bool? IsFulfiledOffence { get; set; }
    }
}
