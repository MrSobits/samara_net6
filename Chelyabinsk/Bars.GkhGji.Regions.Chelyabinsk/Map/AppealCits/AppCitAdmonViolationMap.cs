namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для "Обращениям граждан - Предостережение - Нарушение"</summary>
    public class AppCitAdmonViolationMap : BaseEntityMap<AppCitAdmonVoilation>
    {

        public AppCitAdmonViolationMap() :
                base("Обращениям граждан - Предостережение - Нарушение", "GJI_CH_APPCIT_ADMON_VIOLATION")
        {
        }

        protected override void Map()
        {
            Property(x => x.FactDate, "Дата фактического устранения").Column("FACT_DATE");
            Property(x => x.PlanedDate, "Планируемая дата устранения").Column("PLANED_DATE");
            Reference(x => x.AppealCitsAdmonition, "Предостережение").Column("APPCIT_ADMONITION_ID").NotNull().Fetch();
            Reference(x => x.ViolationGji, "Нарушение").Column("VIOLATION_ID").NotNull().Fetch();
        }
    }
}
