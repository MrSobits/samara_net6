namespace Bars.Gkh.RegOperator.Entities.Period
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// История изменения проверки периода
    /// </summary>
    public class PeriodCloseCheckHistory : PersistentObject
    {
        /// <summary>
        /// Проверка
        /// </summary>
        public virtual PeriodCloseCheck Check { get; set; }

        /// <summary>
        /// Обязательность
        /// </summary>
        public virtual bool IsCritical { get; set; }

        /// <summary>
        /// Изменивший пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public virtual DateTime ChangeDate { get; set; }
    }
}