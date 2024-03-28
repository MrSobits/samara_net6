/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
///     using Entities;
///     using B4.DataAccess;
///     using Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Сведения об оплатах коммунальных услуг"
///     /// </summary>
///     public class InfoAboutPaymentCommunalMap : BaseGkhEntityMap<InfoAboutPaymentCommunal>
///     {
///         public InfoAboutPaymentCommunalMap()
///             : base("DI_DISINFO_RO_PAY_COMMUN")
///         {
///             Map(x => x.CounterValuePeriodStart, "COUNTER_VALUE_START");
///             Map(x => x.CounterValuePeriodEnd, "COUNTER_VALUE_END");
///             Map(x => x.Accrual, "ACCRUAL");
///             Map(x => x.Payed, "PAYED");
///             Map(x => x.Debt, "DEBT");
/// 
///             References(x => x.DisclosureInfoRealityObj, "DISINFO_RO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.InfoAboutPaymentCommunal"</summary>
    public class InfoAboutPaymentCommunalMap : BaseImportableEntityMap<InfoAboutPaymentCommunal>
    {
        
        public InfoAboutPaymentCommunalMap() : 
                base("Bars.GkhDi.Entities.InfoAboutPaymentCommunal", "DI_DISINFO_RO_PAY_COMMUN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.CounterValuePeriodStart, "CounterValuePeriodStart").Column("COUNTER_VALUE_START");
            Property(x => x.CounterValuePeriodEnd, "CounterValuePeriodEnd").Column("COUNTER_VALUE_END");
            Property(x => x.TotalConsumption, "TotalConsumption").Column("TOTAL_CONSUMP");
            Property(x => x.Accrual, "Accrual").Column("ACCRUAL");
            Property(x => x.Payed, "Payed").Column("PAYED");
            Property(x => x.Debt, "Debt").Column("DEBT");
            Property(x => x.AccrualByProvider, "AccrualByProvider").Column("ACCRUAL_BY_PROV");
            Property(x => x.PayedToProvider, "PayedToProvider").Column("PAYED_TO_PROV");
            Property(x => x.DebtToProvider, "DebtToProvider").Column("DEBT_TO_PROV");
            Property(x => x.ReceivedPenaltySum, "ReceivedPenaltySum").Column("RECEIVED_PENALTY");
            
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
