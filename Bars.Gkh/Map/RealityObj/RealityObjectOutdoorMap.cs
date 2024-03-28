namespace Bars.Gkh.Map.RealityObj
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.RealityObj;

    public class RealityObjectOutdoorMap : BaseEntityMap<RealityObjectOutdoor>
    {
        /// <inheritdoc />
        public RealityObjectOutdoorMap()
            : base("Bars.Gkh.Entities.RealityObj.RealityObjectOutdoor", "GKH_REALITY_OBJECT_OUTDOOR")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.MunicipalityFiasOktmo, nameof(RealityObjectOutdoor.MunicipalityFiasOktmo)).Column("MUNICIPALITY_FIAS_OKTMO_ID").NotNull().Fetch();
            this.Property(x => x.Name, nameof(RealityObjectOutdoor.Name)).Column("NAME").Length(255).NotNull();
            this.Property(x => x.Code, nameof(RealityObjectOutdoor.Code)).Column("CODE").Length(255).NotNull();
            this.Property(x => x.Area, nameof(RealityObjectOutdoor.Area)).Column("AREA");
            this.Property(x => x.AsphaltArea, nameof(RealityObjectOutdoor.AsphaltArea)).Column("ASPHALT_AREA");
            this.Property(x => x.Description, nameof(RealityObjectOutdoor.Description)).Column("DESCRIPTION").Length(255);
            this.Property(x => x.RepairPlanYear, nameof(RealityObjectOutdoor.RepairPlanYear)).Column("REPAIR_PLAN_YEAR");
        }
    }
}
