namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Событие для перерасчета
    /// </summary>
    public class PersonalAccountRecalcEvent : BaseEntity
    {
        #region const

        public const string PenaltyType = "Penalty";
        public const string ChargeType = "Charge";

        #endregion

        /// <summary>
        /// ЛС
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Тип перерасчета. Например, Charge или Penalty. 
        /// По этому типу будем отличать для какого перерасчета действует 
        /// </summary>
        public virtual string RecalcType { get; set; }

        /// <summary>
        /// Провайдер даты перерасчета. Например, оплата
        /// </summary>
        public virtual string RecalcProvider { get; set; }

        /// <summary>
        /// Дата, когда произошло событие, которое влияет на перерасчет
        /// </summary>
        public virtual DateTime EventDate { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Тип события для перерасчета
        /// </summary>
        public virtual RecalcEventType RecalcEventType { get; set; }
    }
}