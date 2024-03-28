namespace Bars.Gkh1468.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Связь Поставщик ресурсов с жилым домом
    /// </summary>
    public class PublicServiceOrgRealtyObject : BaseImportableEntity
    {
        /// <summary>
        /// Поставщик ресурсов
        /// </summary>
        public virtual PublicServiceOrg PublicServiceOrg { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}