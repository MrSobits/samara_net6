namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Платежный документ по начислениям и оплатам на КР
    /// </summary>
    public class ConfirmContributionDoc : BaseEntity
    {
        /// <summary>
        /// Учет платежных документов по начислениям и оплатам на КР
        /// </summary>
        public virtual ConfirmContribution ConfirmContribution { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата перечисления
        /// </summary>
        public virtual DateTime? TransferDate { get; set; }

        /// <summary>
        /// Скан перечисления
        /// </summary>
        public virtual FileInfo Scan { get; set; }

        /// <summary>
        /// Сумма платежного поручения
        /// </summary>
        public virtual decimal? Amount { get; set; }
    }
}