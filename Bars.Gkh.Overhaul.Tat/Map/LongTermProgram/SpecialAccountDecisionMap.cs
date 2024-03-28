/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class SpecialAccountDecisionMap : BaseJoinedSubclassMap<SpecialAccountDecision>
///     {
///         public SpecialAccountDecisionMap() : base("OVRHL_PR_DEC_SPEC_ACC", "ID")
///         {
///             Map(x => x.TypeOrganization, "TYPE_ORGANIZATION", true);
///             Map(x => x.AccountNumber, "ACC_NUMBER");
///             Map(x => x.OpenDate, "OPEN_DATE");
///             Map(x => x.CloseDate, "CLOSE_DATE");
/// 
///             Map(x => x.Inn, "INN", false, 50);
///             Map(x => x.Kpp, "KPP", false, 50);
///             Map(x => x.Ogrn, "OGRN", false, 50);
///             Map(x => x.Okpo, "OKPO", false, 50);
///             Map(x => x.Bik, "BIK", false, 50);
///             Map(x => x.CorrAccount, "CORR_ACCOUNT", false, 50);
/// 
///             References(x => x.MailingAddress, "FIAS_MAIL_ADDRESS_ID", ReferenceMapConfig.Fetch | ReferenceMapConfig.CascadeAll);
///             References(x => x.BankHelpFile, "HELP_FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.CreditOrg, "CREDIT_ORG_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RegOperator, "REG_OPERATOR_ID", ReferenceMapConfig.Fetch);
///             References(x => x.ManagingOrganization, "MANAGING_ORG_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.SpecialAccountDecision"</summary>
    public class SpecialAccountDecisionMap : JoinedSubClassMap<SpecialAccountDecision>
    {
        
        public SpecialAccountDecisionMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.SpecialAccountDecision", "OVRHL_PR_DEC_SPEC_ACC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeOrganization, "TypeOrganization").Column("TYPE_ORGANIZATION").NotNull();
            Reference(x => x.RegOperator, "RegOperator").Column("REG_OPERATOR_ID").Fetch();
            Reference(x => x.ManagingOrganization, "ManagingOrganization").Column("MANAGING_ORG_ID").Fetch();
            Property(x => x.AccountNumber, "AccountNumber").Column("ACC_NUMBER").Length(250);
            Property(x => x.OpenDate, "OpenDate").Column("OPEN_DATE");
            Property(x => x.CloseDate, "CloseDate").Column("CLOSE_DATE");
            Reference(x => x.BankHelpFile, "BankHelpFile").Column("HELP_FILE_ID").Fetch();
            Reference(x => x.CreditOrg, "CreditOrg").Column("CREDIT_ORG_ID").Fetch();
            Reference(x => x.MailingAddress, "MailingAddress").Column("FIAS_MAIL_ADDRESS_ID").Fetch();
            Property(x => x.Inn, "Inn").Column("INN").Length(50);
            Property(x => x.Kpp, "Kpp").Column("KPP").Length(50);
            Property(x => x.Ogrn, "Ogrn").Column("OGRN").Length(50);
            Property(x => x.Okpo, "Okpo").Column("OKPO").Length(50);
            Property(x => x.Bik, "Bik").Column("BIK").Length(50);
            Property(x => x.CorrAccount, "CorrAccount").Column("CORR_ACCOUNT").Length(50);
        }
    }
}
