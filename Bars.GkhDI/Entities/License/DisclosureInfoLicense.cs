namespace Bars.GkhDi.Entities
{
    using System;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Раскрытыие информации о лицензиях управляющей организаци (УО), согласно 988.
    /// </summary>
    public class DisclosureInfoLicense : BaseImportableEntity
    {
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Номер лицензии
        /// </summary>
        public virtual string LicenseNumber { get; set; }

        /// <summary>
        /// Дата получения лицензии
        /// </summary>
        public virtual DateTime DateReceived { get; set; }


        /// <summary>
        /// Орган, выдавший лицензию
        /// </summary>
        public virtual string LicenseOrg { get; set; }

        /// <summary>
        /// Документ лицензии
        /// </summary>
        public virtual FileInfo LicenseDoc { get; set; }
    }


}
