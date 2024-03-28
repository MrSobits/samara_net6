/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Сведения о случаях снижения платы"
///     /// </summary>
///     public class InfoAboutReductionPaymentMap : BaseGkhEntityMap<InfoAboutReductionPayment>
///     {
///         public InfoAboutReductionPaymentMap()
///             : base("DI_DISINFO_RO_REDUCT_PAY")
///         {
///             Map(x => x.ReasonReduction, "REASON_REDUCTION").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.RecalculationSum, "RECALC_SUM");
///             Map(x => x.OrderDate, "ORDER_DATE");
///             Map(x => x.OrderNum, "ORDER_NUM").Length(50);
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
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.InfoAboutReductionPayment"</summary>
    public class InfoAboutReductionPaymentMap : BaseImportableEntityMap<InfoAboutReductionPayment>
    {
        
        public InfoAboutReductionPaymentMap() : 
                base("Bars.GkhDi.Entities.InfoAboutReductionPayment", "DI_DISINFO_RO_REDUCT_PAY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.ReasonReduction, "ReasonReduction").Column("REASON_REDUCTION").Length(300);
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            Property(x => x.RecalculationSum, "RecalculationSum").Column("RECALC_SUM");
            Property(x => x.OrderDate, "OrderDate").Column("ORDER_DATE");
            Property(x => x.OrderNum, "OrderNum").Column("ORDER_NUM").Length(50);
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
