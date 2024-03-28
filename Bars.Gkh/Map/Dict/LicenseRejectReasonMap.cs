namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг для Причина отказа
    /// </summary>
    public class LicenseRejectReasonMap : BaseImportableEntityMap<LicenseRejectReason>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public LicenseRejectReasonMap()
            : base("Bars.Gkh.Entities.LicenseRejectReason", "GKH_LICENSE_REJECT_REASON")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
        }
    }
}