namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Период начислений"</summary>
    public class ChargePeriodMap : BaseImportableEntityMap<ChargePeriod>
    {
        
        public ChargePeriodMap() : 
                base("Период начислений", "REGOP_PERIOD")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование периода").Column("PERIOD_NAME").Length(250).NotNull();
            this.Property(x => x.StartDate, "Дата открытия периода").Column("CSTART").NotNull();
            this.Property(x => x.EndDate, "Дата закрытия периода").Column("CEND");
            this.Property(x => x.IsClosed, "Флаг: период закрыт").Column("CIS_CLOSED").NotNull();
            this.Property(x => x.IsClosing, "Признак, что период закрывается").Column("IS_CLOSING").NotNull();
        }
    }
}
