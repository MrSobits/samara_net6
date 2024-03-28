namespace Bars.Gkh1468.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Связь Поставщик коммунальных услуг с МО
    /// </summary>
    public class PublicServiceOrgMunicipality : BaseImportableEntity
    {
        /// <summary>
        /// Поставщик ком. услуг
        /// </summary>
        public virtual PublicServiceOrg PublicServiceOrg { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}