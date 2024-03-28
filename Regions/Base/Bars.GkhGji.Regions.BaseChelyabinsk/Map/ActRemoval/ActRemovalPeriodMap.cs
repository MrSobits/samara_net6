namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.ActRemoval
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    /// <summary>Маппинг для "Дата и время проведения проверки предписания"</summary>
    public class ActRemovalPeriodMap : BaseEntityMap<ActRemovalPeriod>
    {
		public ActRemovalPeriodMap() : 
                base("Дата и время проведения проверки предписания", "GJI_NSO_ACTREMOVAL_PERIOD")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DateCheck, "Дата").Column("DATE_CHECK");
            this.Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            this.Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            this.Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
        }
    }
}
