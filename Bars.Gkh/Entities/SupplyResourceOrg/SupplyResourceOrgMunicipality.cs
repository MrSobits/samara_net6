namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь Поставщика коммунальных услуг с МО
    /// </summary>
    public class SupplyResourceOrgMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Поставщик ресурсов
        /// </summary>
        public virtual SupplyResourceOrg SupplyResourceOrg { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}