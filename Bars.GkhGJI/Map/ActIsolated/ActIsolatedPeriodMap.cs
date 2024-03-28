namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Дата и время проведения проверки акта без взаимодействия"</summary>
    public class ActIsolatedPeriodMap : BaseEntityMap<ActIsolatedPeriod>
    {
        public ActIsolatedPeriodMap() : 
                base("Дата и время проведения проверки акта без взаимодействия", "GJI_ACTISOLATED_PERIOD")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DateCheck, "Дата").Column("DATE_CHECK");
            this.Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            this.Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            this.Reference(x => x.ActIsolated, "Акт без взаимодействия").Column("ACTISOLATED_ID").NotNull().Fetch();
        }
    }
}
