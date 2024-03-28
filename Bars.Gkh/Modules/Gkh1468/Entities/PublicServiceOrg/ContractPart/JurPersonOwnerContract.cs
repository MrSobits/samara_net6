namespace Bars.Gkh.Modules.Gkh1468.Entities.ContractPart
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Сторона договора "Юридическое лицо"
    /// </summary>
    public class JurPersonOwnerContract : BaseContractPart
    {
        /// <summary>
        /// Лицо, являющееся стороной договора
        /// </summary>
        public virtual TypeContactPerson TypeContactPerson { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}
