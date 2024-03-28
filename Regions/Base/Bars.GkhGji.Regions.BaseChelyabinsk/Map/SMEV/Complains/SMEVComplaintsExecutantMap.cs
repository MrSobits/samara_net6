namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVComplaintsExecutantMap : BaseEntityMap<SMEVComplaintsExecutant>
    {
        
        public SMEVComplaintsExecutantMap() : 
                base("", "SMEV_CH_COMPLAINTS_EXECUTANT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.OrderDate, "OrderDate").Column("ORDER_DATE");
            this.Property(x => x.PerformanceDate, "PerformanceDate").Column("PERFOM_DATE");
            this.Property(x => x.IsResponsible, "IsResponsible").Column("RESPONSIBLE");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION").Length(255);
            this.Reference(x => x.SMEVComplaints, "AppealCits").Column("COMPLAINT_ID").NotNull().Fetch();
            this.Reference(x => x.Executant, "Executant").Column("EXECUTANT_ID").Fetch();
            this.Reference(x => x.Author, "Author").Column("AUTHOR_ID").Fetch();
        }
    }
}
