namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь поставщика жил. услуг с МО
    /// </summary>
    public class ServiceOrgMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Поставщик жил. услуг
        /// </summary>
        public virtual ServiceOrganization ServOrg { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}