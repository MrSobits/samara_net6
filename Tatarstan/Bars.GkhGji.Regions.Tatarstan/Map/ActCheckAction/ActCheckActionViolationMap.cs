namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckActionViolationMap : BaseEntityMap<ActCheckActionViolation>
    {
        public ActCheckActionViolationMap()
            : base("Нарушение действия акта проверки", "GJI_ACTCHECK_ACTION_VIOLATION")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Violation, "Нарушение").Column("GJI_DICT_VIOLATION_ID");
            this.Property(x => x.ContrPersResponse, "Пояснение контролируемого лица").Column("CONTR_PERS_RESPONSE");
            
            this.Reference(x => x.ActCheckAction, "Действие акта проверки").Column("ACTCHECK_ACTION_ID");
        }
    }
}