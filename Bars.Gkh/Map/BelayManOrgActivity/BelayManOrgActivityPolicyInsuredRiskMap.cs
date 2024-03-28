/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     /// <summary>
///     /// Маппинг сущности "Застрахованные риски"
///     /// </summary>
///     public class BelayManOrgActivityPolicyInsuredRiskMap : BaseGkhEntityMap<Entities.BelayPolicyRisk>
///     {
///         public BelayManOrgActivityPolicyInsuredRiskMap() : base("GKH_BELAY_POLICY_RISK")
///         {
///             References(x => x.KindRisk, "KIND_RISK_ID").Not.Nullable().Fetch.Join();
/// 
///             References(x => x.BelayPolicy, "BELAY_POLICY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Застрахованный риск"</summary>
    public class BelayPolicyRiskMap : BaseImportableEntityMap<BelayPolicyRisk>
    {
        
        public BelayPolicyRiskMap() : 
                base("Застрахованный риск", "GKH_BELAY_POLICY_RISK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.KindRisk, "Виды рисков").Column("KIND_RISK_ID").NotNull().Fetch();
            Reference(x => x.BelayPolicy, "Страховой полис").Column("BELAY_POLICY_ID").NotNull().Fetch();
        }
    }
}
