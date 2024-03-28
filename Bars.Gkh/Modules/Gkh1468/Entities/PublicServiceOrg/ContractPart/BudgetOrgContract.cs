namespace Bars.Gkh.Modules.Gkh1468.Entities.ContractPart
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities.Dict;

    /// <summary>
    /// Сторона договора "Бюджетная организация"
    /// </summary>
    public class BudgetOrgContract : BaseContractPart
    {
        /// <summary>
        /// Организация
        /// </summary>
        public virtual Contragent Organization { get; set; }

        /// <summary>
        /// Вид потребителя
        /// </summary>       
        public virtual TypeCustomer TypeCustomer { get; set; }
    }
}