namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Услуга Поставщика коммунальных услуг
    /// </summary>
    public class SupplyResourceOrgService : BaseGkhEntity
    {
        /// <summary>
        /// Поставщик коммунальных услуг
        /// </summary>
        public virtual SupplyResourceOrg SupplyResourceOrg { get; set; }

        /// <summary>
        /// Тип обслуживания
        /// </summary>
        public virtual TypeService TypeService { get; set; }
    }
}