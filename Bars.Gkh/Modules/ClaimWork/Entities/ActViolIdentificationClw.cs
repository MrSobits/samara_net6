namespace Bars.Gkh.ClaimWork.Entities
{
    using System;
    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Enums;

    /// <summary>
    /// Акт выявления нарушений
    /// </summary>
    public class ActViolIdentificationClw : DocumentClw
    {
        /// <summary>
        /// Тип акта
        /// </summary>
        public virtual ActViolIdentificationType ActType { get; set;  }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public virtual DateTime? SendDate { get; set; }

        /// <summary>
        /// Факт получения
        /// </summary>
        public virtual FactOfReceiving FactOfReceiving { get; set; }

        /// <summary>
        /// Факт подписания
        /// </summary>
        public virtual FactOfSigning FactOfSigning { get; set; }

        /// <summary>
        /// Дата подписания
        /// </summary>
        public virtual DateTime? SignDate { get; set; }

        /// <summary>
        /// Время подписания
        /// </summary>
        public virtual string SignTime { get; set; }

        /// <summary>
        /// Место подписания
        /// </summary>
        public virtual string SignPlace { get; set; }

        /// <summary>
        /// Дата составления
        /// </summary>
        public virtual DateTime? FormDate { get; set; }

        /// <summary>
        /// Время составления
        /// </summary>
        public virtual string FormTime { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        public virtual string FormPlace { get; set; }

        /// <summary>
        /// Лица, присутствующий при составлении
        /// </summary>
        public virtual string Persons { get; set; }
    }
}