namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Обращениям граждан - Предостережение - Нарушение"</summary>
    public class AppCitPrFondViolationMap : BaseEntityMap<AppCitPrFondVoilation>
    {
        
        public AppCitPrFondViolationMap() : 
                base("Обращениям граждан - Предписание ФКР - Нарушение", "GJI_APPCIT_PR_FOND_VIOLATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.FactDate, "Дата фактического устранения").Column("FACT_DATE");
            Property(x => x.PlanedDate, "Планируемая дата устранения").Column("PLANED_DATE");
            Reference(x => x.AppealCitsPrescriptionFond, "Предписание ФКР").Column("APPCIT_PR_FOND_ID").NotNull().Fetch();
            Reference(x => x.ViolationGji, "Нарушение").Column("VIOLATION_ID").NotNull().Fetch();
        }
    }
}
