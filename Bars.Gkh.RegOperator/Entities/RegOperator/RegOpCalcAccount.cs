namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Расчетные счета Рег оператора
    /// </summary>
    [Obsolete("Use RegopCalcAccount")]
    public class RegOpCalcAccount : BankAccount
    {
        /// <summary>
        /// Региональный оператор
        /// </summary>
        public virtual RegOperator RegOperator { get; set; }

        /// <summary>
        /// Кредитная организация
        /// </summary>
        //public virtual CreditOrg CreditOrg { get; set; }

        public virtual bool IsSpecial { get; set; }

        /// <summary>
        /// Входящее сальдо 
        /// </summary>
        public virtual decimal? BalanceOut { get; set; }

        /// <summary>
        /// Исходящее сальдо 
        /// </summary>
        public virtual decimal? BalanceIncome { get; set; }

        /// <summary>
        /// Расчетный счет контрагента
        /// </summary>
        public virtual ContragentBankCreditOrg ContragentBankCrOrg { get; set; }
    }
}