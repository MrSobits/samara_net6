namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Должностное лицо лицензии
    /// </summary>
    public class ManOrgLicensePerson : BaseImportableEntity
    {
        /// <summary>
        /// Лицензия
        /// </summary>
        public virtual ManOrgLicense ManOrgLicense { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual Person Person { get; set; }

    }
}
