namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckActionInspectorMap : BaseEntityMap<ActCheckActionInspector>
    {
        public ActCheckActionInspectorMap()
            : base("Инспектор действия акта проверки", "GJI_ACTCHECK_ACTION_INSPECTOR")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ActCheckAction, "Действие акта проверки").Column("ACTCHECK_ACTION_ID");
            this.Reference(x => x.Inspector, "Инспекор").Column("INSPECTOR_ID");
        }
    }
}