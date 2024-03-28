namespace Bars.Gkh.RegOperator.Entities.Period
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Entities;

    public class PeriodCloseRollbackHistory : BaseEntity
    {
        /// <summary>
        /// Наименование периода
        /// </summary>
        public virtual string PeriodName { get; set; }

        /// <summary>
        /// Изменивший пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Дата отката
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Период на который откатили
        /// </summary>
        public virtual ChargePeriod Period { get; set; }
    }
}