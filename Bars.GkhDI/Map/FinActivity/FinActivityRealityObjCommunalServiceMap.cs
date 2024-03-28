/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Коммунальные услуги финансовой деятельности в доме"
///     /// </summary>
///     public class FinActivityRealityObjCommunalServiceMap : BaseGkhEntityMap<FinActivityRealityObjCommunalService>
///     {
///         public FinActivityRealityObjCommunalServiceMap()
///             : base("DI_DISINFO_FINCOMMUN_RO")
///         {
///             Map(x => x.TypeServiceDi, "TYPE_SERVICE_DI").Not.Nullable().CustomType<TypeServiceDi>();
///             Map(x => x.PaidOwner, "PAID_OWNER");
///             Map(x => x.DebtOwner, "DEBT_OWNER");
///             Map(x => x.PaidByIndicator, "PAID_BY_INDICATOR");
///             Map(x => x.PaidByAccount, "PAID_BY_ACCOUNT");
/// 
///             References(x => x.DisclosureInfoRealityObj, "DISINFO_RO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivityRealityObjCommunalService"</summary>
    public class FinActivityRealityObjCommunalServiceMap : BaseImportableEntityMap<FinActivityRealityObjCommunalService>
    {
        
        public FinActivityRealityObjCommunalServiceMap() : 
                base("Bars.GkhDi.Entities.FinActivityRealityObjCommunalService", "DI_DISINFO_FINCOMMUN_RO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeServiceDi, "TypeServiceDi").Column("TYPE_SERVICE_DI").NotNull();
            Property(x => x.PaidOwner, "PaidOwner").Column("PAID_OWNER");
            Property(x => x.DebtOwner, "DebtOwner").Column("DEBT_OWNER");
            Property(x => x.PaidByIndicator, "PaidByIndicator").Column("PAID_BY_INDICATOR");
            Property(x => x.PaidByAccount, "PaidByAccount").Column("PAID_BY_ACCOUNT");
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
        }
    }
}
