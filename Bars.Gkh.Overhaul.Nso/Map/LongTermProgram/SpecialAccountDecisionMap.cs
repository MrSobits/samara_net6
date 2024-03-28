/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class SpecialAccountDecisionMap : BaseJoinedSubclassMap<SpecialAccountDecision>
///     {
///         public SpecialAccountDecisionMap()
///             : base("OVRHL_PR_DEC_SPEC_ACC", "ID")
///         {
///             Map(x => x.TypeOrganization, "TYPE_ORGANIZATION", true);
///             Map(x => x.AccountNumber, "ACC_NUMBER");
///             Map(x => x.OpenDate, "OPEN_DATE");
///             Map(x => x.CloseDate, "CLOSE_DATE");
/// 
///             References(x => x.BankHelpFile, "HELP_FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.CreditOrg, "CREDIT_ORG_ID", ReferenceMapConfig.Fetch);
///            // References(x => x.RegOperator, "REG_OPERATOR_ID", ReferenceMapConfig.Fetch);
///             References(x => x.ManagingOrganization, "MANAGING_ORG_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.SpecialAccountDecision"</summary>
    public class SpecialAccountDecisionMap : JoinedSubClassMap<SpecialAccountDecision>
    {
        
        public SpecialAccountDecisionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.SpecialAccountDecision", "OVRHL_PR_DEC_SPEC_ACC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeOrganization, "TypeOrganization").Column("TYPE_ORGANIZATION").NotNull();
            Reference(x => x.ManagingOrganization, "ManagingOrganization").Column("MANAGING_ORG_ID").Fetch();
            Property(x => x.AccountNumber, "AccountNumber").Column("ACC_NUMBER").Length(250);
            Property(x => x.OpenDate, "OpenDate").Column("OPEN_DATE");
            Property(x => x.CloseDate, "CloseDate").Column("CLOSE_DATE");
            Reference(x => x.BankHelpFile, "BankHelpFile").Column("HELP_FILE_ID").Fetch();
            Reference(x => x.CreditOrg, "CreditOrg").Column("CREDIT_ORG_ID").Fetch();
        }
    }
}
