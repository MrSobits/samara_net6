namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Специальный счет
    /// </summary>
    public class SpecialAccount : BankAccount
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Кредитная организация
        /// </summary>
        public virtual CreditOrg CreditOrganization { get; set; }

        /// <summary>
        /// Владелец специального счета
        /// </summary>
        public virtual Contragent AccountOwner {get;set;}

        /// <summary>
        /// Решение по спец счету
        /// </summary>
        public virtual SpecialAccountDecision Decision { get; set; }
    }
}
