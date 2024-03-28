namespace Bars.GkhGji.Regions.Zabaykalye.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities.AppealCits;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Zabaykalye.Entities.AppealCitsExecutant"</summary>
    public class AppealCitsExecutantMap : BaseEntityMap<AppealCitsExecutant>
    {
        
        public AppealCitsExecutantMap() : 
                base("Bars.GkhGji.Regions.Zabaykalye.Entities.AppealCitsExecutant", "GJI_APPCIT_EXECUTANT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OrderDate, "OrderDate").Column("ORDER_DATE");
            Property(x => x.PerformanceDate, "PerformanceDate").Column("PERFOM_DATE");
            Property(x => x.IsResponsible, "IsResponsible").Column("RESPONSIBLE");
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(255);
            Reference(x => x.AppealCits, "AppealCits").Column("APPCIT_ID").NotNull().Fetch();
            Reference(x => x.Executant, "Executant").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Author, "Author").Column("AUTHOR_ID").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
