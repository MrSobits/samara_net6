namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг для Причина переоформления лицензии
    /// </summary>
    public class LicenseRegistrationReasonMap : BaseImportableEntityMap<LicenseRegistrationReason>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public LicenseRegistrationReasonMap()
            : base("Bars.Gkh.Entities.LicenseRegistrationReason", "GKH_LICENSE_REGISTRATION_REASON")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
        }
    }
}