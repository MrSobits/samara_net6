namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг для "Этап выявления нарушения в доме для акта без взаимодействия"
    /// </summary>
    public class ActIsolatedRealObjViolationMap : JoinedSubClassMap<ActIsolatedRealObjViolation>
    {
        public ActIsolatedRealObjViolationMap() : 
                base("Этап выявления нарушения в доме для акта без взаимодействия", "GJI_ACTISOLATED_ROBJECT_VIOLATION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.EventResult, "Результаты проведения мероприятия").Column("EVENT_RESULT").Length(500);
            this.Reference(x => x.ActIsolatedRealObj, "Дом акта без взаимодействия").Column("ACTISOLATED_RO_ID").NotNull().Fetch();
        }
    }
}