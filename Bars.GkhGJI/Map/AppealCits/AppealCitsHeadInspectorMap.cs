namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class AppealCitsHeadInspectorMap: BaseEntityMap<AppealCitsHeadInspector>
    {
        public AppealCitsHeadInspectorMap()
            : base("Руководитель обращения", "GJI_APPCIT_HEADINSP")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.AppealCits, "Обращение").Column("APPCIT_ID").NotNull();
            this.Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
        }
    }
}