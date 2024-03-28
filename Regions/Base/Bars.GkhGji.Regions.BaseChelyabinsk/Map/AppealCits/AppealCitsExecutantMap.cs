namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.AppealCits
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.AppealCitsExecutant"</summary>
    public class AppealCitsExecutantMap : BaseEntityMap<AppealCitsExecutant>
    {
        
        public AppealCitsExecutantMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.AppealCitsExecutant", "GJI_APPCIT_EXECUTANT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.OrderDate, "OrderDate").Column("ORDER_DATE");
            this.Property(x => x.PerformanceDate, "PerformanceDate").Column("PERFOM_DATE");
            this.Property(x => x.IsResponsible, "IsResponsible").Column("RESPONSIBLE");
            this.Property(x => x.OnApproval, "OnApproval").Column("ONAPPROVAL");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION").Length(255);
            this.Reference(x => x.AppealCits, "AppealCits").Column("APPCIT_ID").NotNull().Fetch();
            this.Reference(x => x.Executant, "Executant").Column("EXECUTANT_ID").Fetch();
            this.Reference(x => x.Author, "Author").Column("AUTHOR_ID").Fetch();
            this.Reference(x => x.Controller, "Controller").Column("CONTROLLER_ID").Fetch();
            this.Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            this.Reference(x => x.Resolution, "Resolution").Column("RESOLUTION_ID").Fetch();
            this.Reference(x => x.ZonalInspection, "ZonalInspection").Column("ZONAINSP_ID").Fetch();
        }
    }
}