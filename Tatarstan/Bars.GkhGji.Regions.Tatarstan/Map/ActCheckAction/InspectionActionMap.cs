namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InspectionAction;

    public class InspectionActionMap : JoinedSubClassMap<InspectionAction>
    {
        public InspectionActionMap()
            : base("Действие акта проверки с типом \"Осмотр\"", "GJI_ACTCHECK_INSPECTION_ACTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ContinueDate, "Дата продолжения").Column("CONTINUE_DATE");
            this.Property(x => x.ContinueStartTime, "Время начала продолжения").Column("CONTINUE_START_TIME");
            this.Property(x => x.ContinueEndTime, "Время окончания продолжения").Column("CONTINUE_END_TIME");
            this.Property(x => x.HasViolation, "Нарушения выявлены?").Column("HAS_VIOLATION");
            this.Property(x => x.HasRemark, "Замечания?").Column("HAS_REMARK");
        }
    }
}