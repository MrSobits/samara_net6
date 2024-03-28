/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CreditOrganizationDecisionMap : BaseJoinedSubclassMap<CreditOrganizationDecision>
///     {
///         public CreditOrganizationDecisionMap()
///             : base("OVRHL_PR_DEC_CREDIT_ORG", "ID")
///         {
///             Map(x => x.SettlementAccount, "SETTL_ACCOUNT");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
/// 
///             References(x => x.CreditOrganization, "CREDIT_ORG_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.CreditOrganizationDecision"</summary>
    public class CreditOrganizationDecisionMap : JoinedSubClassMap<CreditOrganizationDecision>
    {
        
        public CreditOrganizationDecisionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.CreditOrganizationDecision", "OVRHL_PR_DEC_CREDIT_ORG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.CreditOrganization, "CreditOrganization").Column("CREDIT_ORG_ID").Fetch();
            Property(x => x.SettlementAccount, "SettlementAccount").Column("SETTL_ACCOUNT").Length(250);
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
        }
    }
}
