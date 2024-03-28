/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
///     using Entities;
///     using B4.DataAccess;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Сведения об оплатах жилищных услуг"
///     /// </summary>
///     public class InfoAboutPaymentHousingMap : BaseGkhEntityMap<InfoAboutPaymentHousing>
///     {
///         public InfoAboutPaymentHousingMap()
///             : base("DI_DISINFO_RO_PAY_HOUSING")
///         {
///             Map(x => x.CounterValuePeriodStart, "COUNTER_VALUE_START");
///             Map(x => x.CounterValuePeriodEnd, "COUNTER_VALUE_END");
///             Map(x => x.GeneralAccrual, "GENERAL_ACCRAUL");
///             Map(x => x.Collection, "COLLECTION");
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
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.InfoAboutPaymentHousing"</summary>
    public class InfoAboutPaymentHousingMap : BaseImportableEntityMap<InfoAboutPaymentHousing>
    {
        
        public InfoAboutPaymentHousingMap() : 
                base("Bars.GkhDi.Entities.InfoAboutPaymentHousing", "DI_DISINFO_RO_PAY_HOUSING")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.CounterValuePeriodStart, "CounterValuePeriodStart").Column("COUNTER_VALUE_START");
            Property(x => x.CounterValuePeriodEnd, "CounterValuePeriodEnd").Column("COUNTER_VALUE_END");
            Property(x => x.GeneralAccrual, "GeneralAccrual").Column("GENERAL_ACCRAUL");
            Property(x => x.Collection, "Collection").Column("COLLECTION");
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
