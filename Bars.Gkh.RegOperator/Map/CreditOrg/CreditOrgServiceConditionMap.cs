/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class CreditOrgServiceConditionMap : BaseImportableEntityMap<CreditOrgServiceCondition>
///     {
///         public CreditOrgServiceConditionMap()
///             : base("REGOP_CREDITORG_SERVICE_COND")
///         {
///             References(x => x.CreditOrg, "CREDITORG_ID", ReferenceMapConfig.Fetch);
/// 
///             Map(x => x.CashServiceSize, "CASH_SERVICE_SIZE", true);
///             Map(x => x.CashServiceDateFrom, "CASH_SERVICE_FROM", true);
///             Map(x => x.CashServiceDateTo, "CAST_SERVICE_TO", false);
///             Map(x => x.OpeningAccPay, "OPENING_ACC_PAY", true);
///             Map(x => x.OpeningAccDateFrom, "OPENING_ACC_FROM", true);
///             Map(x => x.OpeningAccDateTo, "OPENING_ACC_TO", false);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Условия обслуживания кредитными организациями"</summary>
    public class CreditOrgServiceConditionMap : BaseImportableEntityMap<CreditOrgServiceCondition>
    {
        
        public CreditOrgServiceConditionMap() : 
                base("Условия обслуживания кредитными организациями", "REGOP_CREDITORG_SERVICE_COND")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.CreditOrg, "Кредитная организация").Column("CREDITORG_ID").Fetch();
            Property(x => x.CashServiceSize, "Размер расчётно-кассового обслуживания").Column("CASH_SERVICE_SIZE").NotNull();
            Property(x => x.CashServiceDateFrom, "Размер дата \"с\"").Column("CASH_SERVICE_FROM").NotNull();
            Property(x => x.CashServiceDateTo, "Размер дата \"по\"").Column("CAST_SERVICE_TO");
            Property(x => x.OpeningAccPay, "Плата за открытие счета").Column("OPENING_ACC_PAY").NotNull();
            Property(x => x.OpeningAccDateFrom, "Плата дата \"с\"").Column("OPENING_ACC_FROM").NotNull();
            Property(x => x.OpeningAccDateTo, "Плата дата \"по\"").Column("OPENING_ACC_TO");
        }
    }
}
