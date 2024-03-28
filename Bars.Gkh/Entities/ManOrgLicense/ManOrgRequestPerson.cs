namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Должностное лицо заявки на лицензию
    /// </summary>
    public class ManOrgRequestPerson : BaseImportableEntity
    {
        /// <summary>
        /// Заявка на лицензию
        /// </summary>
        public virtual ManOrgLicenseRequest LicRequest { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual Person Person { get; set; }

    }
}
