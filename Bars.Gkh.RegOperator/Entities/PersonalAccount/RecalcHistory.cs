namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// История перерасчета
    /// </summary>
    public class RecalcHistory : BaseEntity
    {
        /// <summary>
        /// ЛС
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Период, в котором считается
        /// </summary>
        public virtual ChargePeriod CalcPeriod { get; set; }

        /// <summary>
        /// Период, для которого происходит перерасчет 
        /// </summary>
        public virtual ChargePeriod RecalcPeriod { get; set; }

        /// <summary>
        /// Сумма перерасчета (дельта)
        /// </summary>
        public virtual decimal RecalcSum { get; set; }

        /// <summary>
        /// Гуид связи с неподтвержденным начислением и боевым 
        /// </summary>
        public virtual string UnacceptedChargeGuid { get; set; }

        /// <summary>
        /// Тип перерасчета
        /// </summary>
        public virtual RecalcType RecalcType { get; set; }
    }
}