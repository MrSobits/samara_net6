/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CalcAccountCreditMap : BaseImportableEntityMap<CalcAccountCredit>
///     {
///         public CalcAccountCreditMap()
///             : base("REGOP_CALC_ACC_CREDIT")
///         {
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.CreditSum, "CREDIT_SUM", true, 0m);
///             Map(x => x.PercentSum, "PERCENT_SUM", true, 0m);
///             Map(x => x.CreditDebt, "CREDIT_DEBT", true, 0m);
///             Map(x => x.PercentDebt, "PERCENT_DEBT", true, 0m);
///             Map(x => x.PercentRate, "PERCENT_RATE", true, 0m);
///             Map(x => x.CreditPeriod, "CREDIT_PERIOD", true, (object) 0);
/// 
///             References(x => x.Account, "ACCOUNT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Document, "DOCUMENT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.RegOperator.Entities;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.CalcAccountCredit"</summary>
    public class CalcAccountCreditMap : BaseImportableEntityMap<CalcAccountCredit>
    {
        
        public CalcAccountCreditMap() : 
                base("Bars.Gkh.RegOperator.Entities.CalcAccountCredit", "REGOP_CALC_ACC_CREDIT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Account, "Расчетный счет").Column("ACCOUNT_ID").NotNull().Fetch();
            Property(x => x.DateStart, "Дата формирования кредита").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата погашения кредита").Column("DATE_END");
            Property(x => x.CreditSum, "Сумма кредита").Column("CREDIT_SUM").DefaultValue(0m).NotNull();
            Property(x => x.PercentSum, "Сумма процентов").Column("PERCENT_SUM").DefaultValue(0m).NotNull();
            Property(x => x.PercentRate, "Процентная ставка").Column("PERCENT_RATE").DefaultValue(0m).NotNull();
            Property(x => x.CreditPeriod, "Срок кредита (месяцев)").Column("CREDIT_PERIOD").DefaultValue(0m).NotNull();
            Property(x => x.CreditDebt, "Сумма основного долга (оставшаяся)").Column("CREDIT_DEBT").DefaultValue(0m).NotNull();
            Property(x => x.PercentDebt, "Сумма долга по процентам").Column("PERCENT_DEBT").DefaultValue(0m).NotNull();
            Reference(x => x.Document, "Документ").Column("DOCUMENT_ID").Fetch();
        }
    }
}
