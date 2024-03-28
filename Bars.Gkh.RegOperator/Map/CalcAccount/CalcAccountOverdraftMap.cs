/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CalcAccountOverdraftMap : BaseImportableEntityMap<CalcAccountOverdraft>
///     {
///         public CalcAccountOverdraftMap()
///             : base("REGOP_CALC_ACC_OVERDRAFT")
///         {
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.OverdraftLimit, "OVERDRAFT_LIMIT", true, 0m);
///             Map(x => x.OverdraftPeriod, "OVERDRAFT_PERIOD", true, (object) 0);
///             Map(x => x.PercentRate, "PERCENT_RATE", true, 0m);
///             Map(x => x.AvailableSum, "AVAILABLE_SUM", true, 0m);
/// 
///             References(x => x.Account, "ACCOUNT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.RegOperator.Entities;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.CalcAccountOverdraft"</summary>
    public class CalcAccountOverdraftMap : BaseImportableEntityMap<CalcAccountOverdraft>
    {
        
        public CalcAccountOverdraftMap() : 
                base("Bars.Gkh.RegOperator.Entities.CalcAccountOverdraft", "REGOP_CALC_ACC_OVERDRAFT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Account, "Расчетный счет").Column("ACCOUNT_ID").NotNull().Fetch();
            Property(x => x.DateStart, "Дата установки").Column("DATE_START").NotNull();
            Property(x => x.OverdraftLimit, "Лимит по овердрафту").Column("OVERDRAFT_LIMIT").DefaultValue(0m).NotNull();
            Property(x => x.PercentRate, "Процентная ставка (день)").Column("PERCENT_RATE").DefaultValue(0m).NotNull();
            Property(x => x.OverdraftPeriod, "Срок беспроцентного овердрафта").Column("OVERDRAFT_PERIOD").DefaultValue(0).NotNull();
            Property(x => x.AvailableSum, "Оставшаяся сумма овердрафта").Column("AVAILABLE_SUM").DefaultValue(0m).NotNull();
        }
    }
}
