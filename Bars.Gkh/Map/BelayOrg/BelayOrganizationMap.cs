/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Страховые организация"
///     /// </summary>
///     public class BelayOrganizationMap : BaseGkhEntityMap<BelayOrganization>
///     {
///         public BelayOrganizationMap()
///             : base("GKH_BELAY_ORGANIZATION")
///         {
///             Map(x => x.OrgStateRole, "ORG_STATE_ROLE").Not.Nullable().CustomType<OrgStateRole>();
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             // деятельность
///             Map(x => x.ActivityDescription, "ACTIVITY_DESCRIPTION").Length(500);
///             Map(x => x.DateTermination, "DATE_TERMINATION");
///             Map(x => x.ActivityGroundsTermination, "ACTIVITY_TERMINATION").Not.Nullable().CustomType<GroundsTermination>();
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Страховые организация"</summary>
    public class BelayOrganizationMap : BaseImportableEntityMap<BelayOrganization>
    {
        
        public BelayOrganizationMap() : 
                base("Страховые организация", "GKH_BELAY_ORGANIZATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.OrgStateRole, "Статус").Column("ORG_STATE_ROLE").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.ActivityDescription, "Описание для деятельности").Column("ACTIVITY_DESCRIPTION").Length(500);
            Property(x => x.DateTermination, "Дата прекращения деятельности").Column("DATE_TERMINATION");
            Property(x => x.ActivityGroundsTermination, "Основание прекращения деятельности").Column("ACTIVITY_TERMINATION").NotNull();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
