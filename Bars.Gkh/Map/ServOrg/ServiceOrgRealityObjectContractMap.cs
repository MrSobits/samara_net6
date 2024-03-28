/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.ServOrg
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Жилой дом договора управляющей организации"
///     /// </summary>
///     public class ServiceOrgRealityObjectContractMap : BaseGkhEntityMap<ServiceOrgRealityObjectContract>
///     {
///         public ServiceOrgRealityObjectContractMap()
///             : base("GKH_SORG_REALOBJ_CONTRACT")
///         {
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ServOrgContract, "SERV_ORG_CONTRACT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Жилой дом договора организации поставщика жил. услуг"</summary>
    public class ServiceOrgRealityObjectContractMap : BaseImportableEntityMap<ServiceOrgRealityObjectContract>
    {
        
        public ServiceOrgRealityObjectContractMap() : 
                base("Жилой дом договора организации поставщика жил. услуг", "GKH_SORG_REALOBJ_CONTRACT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJ_ID").NotNull().Fetch();
            Reference(x => x.ServOrgContract, "Договор").Column("SERV_ORG_CONTRACT_ID").NotNull().Fetch();
        }
    }
}
