namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class EffectivenessAndPerformanceIndexMap : BaseEntityMap<EffectivenessAndPerformanceIndex>
    {
        /// <inheritdoc />
        public EffectivenessAndPerformanceIndexMap()
            : base("Bars.GkhGji.Map.Dict.EffectivenessAndPerformanceIndex", "GJI_DICT_EFFEC_PERF_INDEX")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Code, "Code").Column("CODE").Length(300);
            this.Property(x => x.Name, "Name").Column("NAME").Length(500);
            this.Property(x => x.TorId, "TorId").Column("TOR_ID");
            this.Property(x => x.ParameterName, "ParameterName").Length(500).Column("PARAM_NAME");
            this.Property(x => x.UnitMeasure, "UnitMeasure").Length(300).Column("UNIT_MEASURE");
            this.Reference(x => x.ControlOrganization, "Контрольно-надзорный орган (КНО)").Column("CONTROL_ORG_ID").Fetch();
        }
    }
}
