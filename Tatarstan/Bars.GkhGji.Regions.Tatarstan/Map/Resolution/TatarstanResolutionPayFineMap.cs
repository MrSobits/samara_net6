namespace Bars.GkhGji.Regions.Tatarstan.Map.Resolution
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tatarstan.Entities.Resolution.TatarstanResolutionPayFine"</summary>
    public class TatarstanResolutionPayFineMap : JoinedSubClassMap<TatarstanResolutionPayFine>
    {
        public TatarstanResolutionPayFineMap()
            :
            base("Bars.GkhGji.Regions.Tatarstan.Entities.Resolution.TatarstanResolutionPayFine", "GJI_TATARSTAN_RESOLUTION_PAYFINE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.AdmissionType, "AdmissionType").Column("ADMISSION_TYPE").NotNull().DefaultValue((int)AdmissionType.Unselected);
        }
    }
}