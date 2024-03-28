namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь Поставщика коммунальных услуг с Жилым домом
    /// </summary>
    public class SupplyResourceOrgRealtyObject : BaseImportableEntity
    {
        /// <summary>
        /// Поставщик ресурсов
        /// </summary>
        public virtual SupplyResourceOrg SupplyResourceOrg { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}