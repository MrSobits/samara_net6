namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дата и время проведения проверки"</summary>
    public class PreventiveVisitPeriodMap : BaseEntityMap<PreventiveVisitPeriod>
    {
        
        public PreventiveVisitPeriodMap() : 
                base("Дата и время проведения проверки", "GJI_PREVENTIVE_VISIT_PERIOD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateCheck, "Дата").Column("DATE_CHECK");
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            Reference(x => x.PreventiveVisit, "Акт проверки").Column("VISIT_ID").NotNull().Fetch();
        }
    }
}
