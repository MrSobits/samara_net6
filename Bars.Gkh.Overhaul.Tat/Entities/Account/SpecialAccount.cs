namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Специальный счет
    /// </summary>
    public class SpecialAccount : BankAccount
    {
        /// <summary>
        /// Кредитная организация
        /// </summary>
        public virtual CreditOrg CreditOrganization { get; set; }

        /// <summary>
        /// Владелец специального счета
        /// </summary>
        public virtual Contragent AccountOwner {get;set;}
    }
}
