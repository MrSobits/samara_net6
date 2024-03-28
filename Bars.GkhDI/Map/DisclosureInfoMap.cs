/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Enums;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Деятельность УК"
///     /// </summary>
///     public class DisclosureInfoMap : BaseGkhEntityMap<DisclosureInfo>
///     {
///         public DisclosureInfoMap() : base("DI_DISINFO")
///         {
///             Map(x => x.AdminPersonnel, "ADMIN_PERSONNEL");
///             Map(x => x.Engineer, "ENGINEER");
///             Map(x => x.Work, "WORK");
///             Map(x => x.DismissedAdminPersonnel, "DISMISSED_ADMIN_PERS");
///             Map(x => x.DismissedEngineer, "DISMISSED_ENGINEER");
///             Map(x => x.DismissedWork, "DISMISSED_WORK");
///             Map(x => x.UnhappyEventCount, "UNHAPPY_EVENT_COUNT");
///             Map(x => x.IsCalculation, "IS_CALCULATING").Not.Nullable();
/// 
///             Map(x => x.TerminateContract, "TERMINATE_CONTRACT").CustomType<YesNoNotSet>().Not.Nullable();
///             Map(x => x.MembershipUnions, "MEMBERSHIP_UNIONS").CustomType<YesNoNotSet>().Not.Nullable();
///             Map(x => x.FundsInfo, "FUNDS_INFO").CustomType<YesNoNotSet>().Not.Nullable();
///             Map(x => x.AdminResponsibility, "ADMIN_RESPONSE").CustomType<YesNoNotSet>().Not.Nullable();
///             Map(x => x.SizePayments, "SIZE_PAYMENTS");
///             Map(x => x.ContractsAvailability, "CONTRACT_AVAIL").CustomType<YesNoNotSet>().Not.Nullable();
///             Map(x => x.NumberContracts, "NUMBER_CONTRACTS");
///             Map(x => x.HasLicense, "HAS_LICENSE").CustomType<YesNoNotSet>();
/// 
///             References(x => x.ManagingOrganization, "MANAG_ORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.PeriodDi, "PERIOD_DI_ID").Not.Nullable().Fetch.Join();
///             References(x => x.DocumentWithoutFunds, "FILE_FUND_WITHOUT").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.DisclosureInfo"</summary>
    public class DisclosureInfoMap : BaseImportableEntityMap<DisclosureInfo>
    {
        
        public DisclosureInfoMap() : 
                base("Bars.GkhDi.Entities.DisclosureInfo", "DI_DISINFO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.AdminPersonnel, "AdminPersonnel").Column("ADMIN_PERSONNEL");
            Property(x => x.Engineer, "Engineer").Column("ENGINEER");
            Property(x => x.Work, "Work").Column("WORK");
            Property(x => x.DismissedAdminPersonnel, "DismissedAdminPersonnel").Column("DISMISSED_ADMIN_PERS");
            Property(x => x.DismissedEngineer, "DismissedEngineer").Column("DISMISSED_ENGINEER");
            Property(x => x.DismissedWork, "DismissedWork").Column("DISMISSED_WORK");
            Property(x => x.UnhappyEventCount, "UnhappyEventCount").Column("UNHAPPY_EVENT_COUNT");
            Property(x => x.IsCalculation, "IsCalculation").Column("IS_CALCULATING").NotNull();
            Property(x => x.TerminateContract, "TerminateContract").Column("TERMINATE_CONTRACT").NotNull();
            Property(x => x.MembershipUnions, "MembershipUnions").Column("MEMBERSHIP_UNIONS").NotNull();
            Property(x => x.FundsInfo, "FundsInfo").Column("FUNDS_INFO").NotNull();
            Property(x => x.AdminResponsibility, "AdminResponsibility").Column("ADMIN_RESPONSE").NotNull();
            Property(x => x.SizePayments, "SizePayments").Column("SIZE_PAYMENTS");
            Property(x => x.ContractsAvailability, "ContractsAvailability").Column("CONTRACT_AVAIL").NotNull();
            Property(x => x.NumberContracts, "NumberContracts").Column("NUMBER_CONTRACTS");
            Property(x => x.HasLicense, "HasLicense").Column("HAS_LICENSE").NotNull();
            Reference(x => x.ManagingOrganization, "ManagingOrganization").Column("MANAG_ORG_ID").NotNull().Fetch();
            Reference(x => x.PeriodDi, "PeriodDi").Column("PERIOD_DI_ID").NotNull().Fetch();
            Reference(x => x.DocumentWithoutFunds, "DocumentWithoutFunds").Column("FILE_FUND_WITHOUT").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
