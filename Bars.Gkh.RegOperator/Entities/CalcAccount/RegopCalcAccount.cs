namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Domain.DatabaseMutex;
    using Overhaul.Entities;

    /// <summary>
    /// Расчетный счет регоператора
    /// </summary>
    public class RegopCalcAccount : CalcAccount, IHaveMutexName
    {
        /// <summary>
        /// Расчетный счет (Кредитная организация контрагента)
        /// </summary>
        public virtual ContragentBankCreditOrg ContragentCreditOrg { get; set; }

        /// <summary>
        /// счет является транзитным
        /// </summary>
        public virtual bool IsTransit { get; set; }

        /// <summary>
        /// Получить ключ для блокировки
        /// </summary>
        /// <returns>
        /// Ключ для блокировки
        /// </returns>
        public virtual string GetMutexName()
        {
            return $"Regop_Calc_Account_{this.Id}";
        }
    }
}