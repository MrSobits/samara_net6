namespace Bars.Gkh.RegOperator.Entities.Period
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Результат проверки периода перед закрытием
    /// </summary>
    public class PeriodCloseCheckResult : PersistentObject
    {
        public PeriodCloseCheckResult(PeriodCloseCheck check, ChargePeriod period, User user)
        {
            this.CheckDate = DateTime.Now;
            this.Code = check.Code;
            this.Impl = check.Impl;
            this.CheckState = PeriodCloseCheckStateType.Pending;
            this.IsCritical = check.IsCritical;
            this.Name = check.Name;
            this.Period = period;
            this.User = user;
        }

        protected PeriodCloseCheckResult()
        {
        }

        /// <summary>
        /// Дата время начала проверки
        /// </summary>
        public virtual DateTime CheckDate { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public virtual string Impl { get; set; }

        /// <summary>
        /// Отображаемое имя проверки
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Обязательность
        /// </summary>
        public virtual bool IsCritical { get; set; }

        /// <summary>
        /// Группа ЛС, в которую скидываются ЛС не прошедшие проверку
        /// </summary>
        public virtual PersAccGroup PersAccGroup { get; set; }

        /// <summary>
        /// Проверяемый период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// Состояние проверки
        /// </summary>
        public virtual PeriodCloseCheckStateType CheckState { get; set; }

        /// <summary>
        /// Лог проверки
        /// </summary>
        public virtual FileInfo LogFile { get; set; }

		/// <summary>
		/// Полный лог проверки для каждой записи
		/// </summary>
		public virtual FileInfo FullLogFile { get; set; }

		/// <summary>
		/// Пользователь, инициировавший проверку
		/// </summary>
		public virtual User User { get; set; }
    }
}