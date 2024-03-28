namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction;

    public class InstrExamActionMap : JoinedSubClassMap<InstrExamAction>
    {
        public InstrExamActionMap()
            : base("Действие акта проверки с типом \"Инструментальное обследование\"", "GJI_ACTCHECK_INSTR_EXAM_ACTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Territory, "Территория").Column("TERRITORY");
            this.Property(x => x.Premise, "Помещение").Column("PREMISE");
            this.Property(x => x.TerritoryAccessDenied, "Отказано в доступе на территорию").Column("TERRITORY_ACCESS_DENIED");
            this.Property(x => x.HasViolation, "Нарушения выявлены?").Column("HAS_VIOLATION");
            this.Property(x => x.UsingEquipment, "Используемое оборудование").Column("USING_EQUIPMENT");
            this.Property(x => x.HasRemark, "Замечания?").Column("HAS_REMARK");
        }
    }
}