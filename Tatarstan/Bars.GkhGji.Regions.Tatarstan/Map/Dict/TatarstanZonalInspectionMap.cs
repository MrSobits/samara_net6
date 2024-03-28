namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class TatarstanZonalInspectionMap : JoinedSubClassMap<TatarstanZonalInspection>
    {
        /// <inheritdoc />
        public TatarstanZonalInspectionMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.Dict.TatarstanZonalInspection", "GKH_DICT_TAT_ZONAINSP")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ControlOrganization, "ControlOrganization").Column("control_org_id");
        }
    }
}
