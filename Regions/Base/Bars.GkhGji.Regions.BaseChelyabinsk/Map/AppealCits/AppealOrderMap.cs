namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.AppealCits
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.AppealCitsExecutant"</summary>
    public class AppealOrderMap : BaseEntityMap<AppealOrder>
    {
        
        public AppealOrderMap() : 
                base("Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealOrder", "GJI_APPCIT_ORDER")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.OrderDate, "OrderDate").Column("ORDER_DATE");
            this.Property(x => x.PerformanceDate, "PerformanceDate").Column("PERFOM_DATE");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION").Length(20000);
            this.Property(x => x.AppealText, "AppealText").Column("APPEAL_TEXT").Length(20000);
            this.Property(x => x.Person, "Person").Column("PERSON").Length(500);
            this.Property(x => x.PersonPhone, "Person").Column("PERSON_PHONE").Length(500);
            this.Reference(x => x.AppealCits, "AppealCits").Column("APPCIT_ID").NotNull().Fetch();
            this.Reference(x => x.Executant, "Executant").Column("CONTRAGENT_ID").NotNull().Fetch();
            this.Property(x => x.YesNoNotSet, "YesNoNotSet").Column("EXECUTED");
            this.Property(x => x.Confirmed, "Confirmed").Column("CONFIRMED");
            this.Property(x => x.ConfirmedGJI, "ConfirmedGJI").Column("CONFIRMED_GJI");
        }
    }
}