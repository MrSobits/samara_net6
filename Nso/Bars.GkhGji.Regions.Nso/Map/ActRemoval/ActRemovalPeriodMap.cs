namespace Bars.GkhGji.Regions.Nso.Map
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Regions.Nso.Entities;

	/// <summary>Маппинг для "Дата и время проведения проверки предписания"</summary>
    public class ActRemovalPeriodMap : BaseEntityMap<ActRemovalPeriod>
    {
		public ActRemovalPeriodMap() : 
                base("Дата и время проведения проверки предписания", "GJI_NSO_ACTREMOVAL_PERIOD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateCheck, "Дата").Column("DATE_CHECK");
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            Reference(x => x.ActRemoval, "Акт проверки предписания").Column("ACTREMOVAL_ID").NotNull().Fetch();
        }
    }
}
