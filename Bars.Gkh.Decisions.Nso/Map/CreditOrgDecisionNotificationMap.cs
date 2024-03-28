/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CreditOrgDecisionNotificationMap : BaseImportableEntityMap<CreditOrgDecisionNotification>
///     {
///         public CreditOrgDecisionNotificationMap() : base("DEC_CREDIT_ORG_NOTIF")
///         {
///             Map(x => x.BankAccountNumber, "BANK_ACC_NUM", false);
///             Map(x => x.Date, "NOTIF_DATE", false);
///             Map(x => x.HasOriginalNotification, "HAS_ORIG_NOTIF");
///             Map(x => x.HasProtocolCopy, "HAS_PROT_COPY");
///             Map(x => x.HasReferenceCopy, "HAS_REF_COPY");
///             Map(x => x.IncomingGjiNumber, "GJI_NUM");
///             Map(x => x.OwnerType, "OWNER_TYPE");
///             Map(x => x.RegistrationDate, "REG_DATE");
///             
///             References(x => x.BankFile, "BANK_FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.CreditOrg, "CR_ORG_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Bars.Gkh.Decisions.Nso.Entities.CreditOrgDecisionNotification"</summary>
    public class CreditOrgDecisionNotificationMap : BaseImportableEntityMap<CreditOrgDecisionNotification>
    {
        
        public CreditOrgDecisionNotificationMap() : 
                base("Bars.Gkh.Decisions.Nso.Entities.CreditOrgDecisionNotification", "DEC_CREDIT_ORG_NOTIF")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.State, "State").Column("STATE_ID").NotNull().Fetch();
            Reference(x => x.CreditOrg, "CreditOrg").Column("CR_ORG_ID").NotNull().Fetch();
            Property(x => x.OwnerType, "OwnerType").Column("OWNER_TYPE");
            Property(x => x.BankAccountNumber, "BankAccountNumber").Column("BANK_ACC_NUM").Length(250);
            Property(x => x.Date, "Date").Column("NOTIF_DATE");
            Property(x => x.RegistrationDate, "RegistrationDate").Column("REG_DATE");
            Reference(x => x.BankFile, "BankFile").Column("BANK_FILE_ID").Fetch();
            Property(x => x.HasOriginalNotification, "HasOriginalNotification").Column("HAS_ORIG_NOTIF");
            Property(x => x.HasReferenceCopy, "HasReferenceCopy").Column("HAS_REF_COPY");
            Property(x => x.HasProtocolCopy, "HasProtocolCopy").Column("HAS_PROT_COPY");
            Property(x => x.IncomingGjiNumber, "IncomingGjiNumber").Column("GJI_NUM").Length(250);
        }
    }
}
