/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     /// <summary>
///     /// Маппинг сущности "Страховой полис МКД"
///     /// </summary>
///     public class BelayManOrgActivityPolicyMkdMap : BaseGkhEntityMap<Entities.BelayPolicyMkd>
///     {
///         public BelayManOrgActivityPolicyMkdMap() : base("GKH_BELAY_POLICY_MKD")
///         {
///             Map(x => x.IsExcluded, "IS_EXCLUDED").Not.Nullable();
/// 
///             References(x => x.BelayPolicy, "BELAY_POLICY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Страховой полис МКД"</summary>
    public class BelayPolicyMkdMap : BaseImportableEntityMap<BelayPolicyMkd>
    {
        
        public BelayPolicyMkdMap() : 
                base("Страховой полис МКД", "GKH_BELAY_POLICY_MKD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.IsExcluded, "Исключен из договора").Column("IS_EXCLUDED").NotNull();
            Reference(x => x.BelayPolicy, "Страховой полис").Column("BELAY_POLICY_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
