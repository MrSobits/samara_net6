/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг сущности "Страховой полис"
///     /// </summary>
///     public class BelayManOrgActivityPolicyMap : BaseGkhEntityMap<Entities.BelayPolicy>
///     {
///         public BelayManOrgActivityPolicyMap() : base("GKH_BELAY_MANORG_POLICY")
///         {
///             Map(x => x.BelaySum, "BELAY_SUM");
///             Map(x => x.Cause, "CAUSE").Length(1000);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentStartDate, "DOCUMENT_START_DATE");
///             Map(x => x.DocumentEndDate, "DOCUMENT_END_DATE");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUM").Length(300);
///             Map(x => x.LimitCivil, "LIMIT_CIVIL");
///             Map(x => x.LimitCivilInsured, "LIMIT_CIVIL_INSURED");
///             Map(x => x.LimitCivilOne, "LIMIT_CIVIL_ONE");
///             Map(x => x.LimitManOrgHome, "LIMIT_MANORG_HOME");
///             Map(x => x.LimitManOrgInsured, "LIMIT_MANORG_INSURED");
///             Map(x => x.PolicyAction, "POLICY_ACTION").Not.Nullable().CustomType<PolicyAction>();
/// 
///             References(x => x.BelayManOrgActivity, "BELAY_MANORG_ACTIVITY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.BelayOrganization, "BELAY_ORGANIZATION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.BelayOrgKindActivity, "BELAY_ORG_KIND_ACTIV_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Страховой полис"</summary>
    public class BelayPolicyMap : BaseImportableEntityMap<BelayPolicy>
    {
        
        public BelayPolicyMap() : 
                base("Страховой полис", "GKH_BELAY_MANORG_POLICY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.BelaySum, "Сумма страхования").Column("BELAY_SUM");
            Property(x => x.Cause, "Причина").Column("CAUSE").Length(1000);
            Property(x => x.DocumentDate, "Дата договора").Column("DOCUMENT_DATE");
            Property(x => x.DocumentStartDate, "Дата начала действия договора").Column("DOCUMENT_START_DATE");
            Property(x => x.DocumentEndDate, "Дата окончания действия договора").Column("DOCUMENT_END_DATE");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.LimitCivil, "Общий лимит гражданской ответственности, в руб.").Column("LIMIT_CIVIL");
            Property(x => x.LimitCivilInsured, "Общий лимит гражданской ответственности (по страховому случаю), в руб.").Column("LIMIT_CIVIL_INSURED");
            Property(x => x.LimitCivilOne, "Общий лимит гражданской ответственности (в отношении одного пострадавшего), в руб" +
                    ".").Column("LIMIT_CIVIL_ONE");
            Property(x => x.LimitManOrgHome, "Лимит ответственности УО (в отношении 1 дома), в руб.").Column("LIMIT_MANORG_HOME");
            Property(x => x.LimitManOrgInsured, "Лимит ответственности УО (по страховому случаю), в руб.").Column("LIMIT_MANORG_INSURED");
            Property(x => x.PolicyAction, "Действие полиса").Column("POLICY_ACTION").NotNull();
            Reference(x => x.BelayManOrgActivity, "Страхование деятельности УО").Column("BELAY_MANORG_ACTIVITY_ID").NotNull().Fetch();
            Reference(x => x.BelayOrganization, "Страховая организация").Column("BELAY_ORGANIZATION_ID").NotNull().Fetch();
            Reference(x => x.BelayOrgKindActivity, "Вид деятельности страховой организации").Column("BELAY_ORG_KIND_ACTIV_ID").NotNull().Fetch();
        }
    }
}
