namespace Bars.Gkh.Modules.Gkh1468.Entities.ContractPart
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Базовый класс - "Сторона договора".
    /// <remarks>Также используется для реализации договора оферты</remarks>
    /// </summary>
    public class BaseContractPart : BaseImportableEntity
    {
        /// <summary>
        /// Договор поставщика ресурсов с домом - с другой стороны
        /// </summary>
        public virtual PublicServiceOrgContract PublicServiceOrgContract { get; set; }

        /// <summary>
        /// Вид договора (стороны договора)
        /// </summary>
        public virtual TypeContractPart TypeContractPart { get; set; }
    }
}
