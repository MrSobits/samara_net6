namespace Bars.Gkh.Overhaul.Entities
{
    using System;
    
    using Bars.Gkh.Entities;

    using Enum;
    using Gkh.Domain.ParameterVersioning;

    public class PaymentSizeCr : BaseImportableEntity, IHasDateActualChange
    {
        /// <summary>
        /// Тип показателя
        /// </summary>
        public virtual TypeIndicator TypeIndicator { get; set; }

        /// <summary>
        /// Размер взноса
        /// </summary>
        public virtual decimal PaymentSize { get; set; }

        /// <summary>
        /// Дата начала периода
        /// </summary>
        public virtual DateTime? DateStartPeriod { get; set; }

        /// <summary>
        /// Дата окончания периода
        /// </summary>
        public virtual DateTime? DateEndPeriod { get; set; }

        DateTime IHasDateActualChange.ActualChangeDate { get { return DateStartPeriod.GetValueOrDefault(); } }
    }
}