namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Dict;


    /// <summary>Маппинг для "Тариф за период для расчета эталонных начислений"</summary>
    public class TariffByPeriodForClaimWorkMap : BaseImportableEntityMap<TariffByPeriodForClaimWork>
    {
        
        public TariffByPeriodForClaimWorkMap() : 
                base("Тариф за период для расчета эталонных начислений", "REGOP_DICT_CLW_TARIF_BY_PERIOD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Value, "Тариф").Column("VALUE");
            Property(x => x.Name, "Наименование тарифа").Column("NAME");
            Reference(x => x.ChargePeriod, "Период").Column("PERIOD_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").Fetch();
        }
    }
}
