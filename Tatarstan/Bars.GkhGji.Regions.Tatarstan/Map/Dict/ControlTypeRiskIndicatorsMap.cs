namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlTypeRiskIndicatorsMap : BaseEntityMap<ControlTypeRiskIndicators>
    {
        /// <inheritdoc />
        public ControlTypeRiskIndicatorsMap()
            : base("Bars.GkhGji.Map.Dict.ControlTypeRiskIndicators", "GJI_DICT_CONTROL_TYPE_RISK_INDICATORS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ControlType, "Вид контроля").Column("CONTROL_TYPE_ID").NotNull();
            this.Property(x => x.Name, "Наименование").Length(5000).Column("NAME");
            this.Property(x => x.ErvkId, "Идентификатор ЕРВК").Column("ERVK_ID");
        }
    }
}