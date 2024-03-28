namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    using Bars.B4.DataAccess;

    public class AccrualsAccountOperation : BaseEntity
    {
        /// <summary>
        /// Счет начислений
        /// </summary>
        public virtual AccrualsAccount Account { get; set; }

        /// <summary>
        /// Дата начисления
        /// </summary>
        public virtual DateTime AccrualDate { get; set; }

        /// <summary>
        /// Итого по дебету
        /// </summary>
        public virtual Double TotalIncome { get; set; }

        /// <summary>
        /// Итого по кредиту
        /// </summary>
        public virtual Double TotalOut { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        public virtual Double OpeningBalance { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public virtual Double ClosingBalance { get; set; }
    }
}
