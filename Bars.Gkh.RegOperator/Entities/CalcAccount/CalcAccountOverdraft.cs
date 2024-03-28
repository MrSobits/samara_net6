namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;

    public class CalcAccountOverdraft : BaseImportableEntity
    {
        /// <summary>
        /// Расчетный счет
        /// </summary>
        public virtual CalcAccount Account { get; set; }

        /// <summary>
        /// Дата установки
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Лимит по овердрафту
        /// </summary>
        public virtual decimal OverdraftLimit { get; set; }

        /// <summary>
        /// Процентная ставка (день)
        /// </summary>
        public virtual decimal PercentRate { get; set; }

        /// <summary>
        /// Срок беспроцентного овердрафта
        /// </summary>
        public virtual int OverdraftPeriod { get; set; }

        /// <summary>
        /// Оставшаяся сумма овердрафта
        /// </summary>
        public virtual decimal AvailableSum { get; set; }
    }
}