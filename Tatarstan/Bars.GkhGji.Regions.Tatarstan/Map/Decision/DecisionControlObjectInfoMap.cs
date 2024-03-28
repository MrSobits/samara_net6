namespace Bars.GkhGji.Regions.Tatarstan.Map.Decision
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    public class DecisionControlObjectInfoMap: BaseEntityMap<DecisionControlObjectInfo>
    {
        /// <inheritdoc />
        public DecisionControlObjectInfoMap()
            : base(nameof(DecisionControlObjectInfo), "GJI_DECISION_CONTROL_OBJECT_INFO")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Decision, "Решение").Column("DECISION_ID").NotNull().Fetch();
            this.Reference(x => x.InspGjiRealityObject, "Проверяемые дома в инспекционной проверки ГЖИ").Column("INSPECTION_ROBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.ControlObjectKind, "Вид объекта контроля").Column("CONTROL_OBJECT_KIND_ID").NotNull().Fetch();
        }
    }
}