namespace Bars.Gkh.Decisions.Nso.Entities
{
    using B4.Modules.FileStorage;

    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Решение о выборе кредитной организации
    /// </summary>
    public class CreditOrgDecision : UltimateDecision
    {
        /// <summary>
        ///     Кредитная организация
        /// </summary>
        public virtual CreditOrg Decision { get; set; }

        /// <summary>
        ///     Банковская информация
        /// </summary>
        public virtual FileInfo BankFile { get; set; }

        /// <summary>
        ///     Номер счета в банке
        /// </summary>
        public virtual string BankAccountNumber { get; set; }
    }
}