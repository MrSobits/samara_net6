/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Поставщик коммунальных услуг"
///     /// </summary>
///     public class SupplyResourceOrgMap : BaseGkhEntityMap<SupplyResourceOrg>
///     {
///         public SupplyResourceOrgMap()
///             : base("GKH_SUPPLY_RESORG")
///         {
///             Map(x => x.OrgStateRole, "ORG_STATE_ROLE").Not.Nullable().CustomType<OrgStateRole>();
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.ActivityGroundsTermination, "ACTIVITY_TERMINATION").Not.Nullable().CustomType<GroundsTermination>();
///             Map(x => x.DescriptionTermination, "DESCRIPTION_TERM").Length(500);
///             Map(x => x.DateTermination, "DATE_TERMINATION");
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
    
    
    /// <summary>Маппинг для "Поставщик коммунальных услуг"</summary>
    public class SupplyResourceOrgMap : BaseImportableEntityMap<SupplyResourceOrg>
    {
        
        public SupplyResourceOrgMap() : 
                base("Поставщик коммунальных услуг", "GKH_SUPPLY_RESORG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.OrgStateRole, "Статус").Column("ORG_STATE_ROLE").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.ActivityGroundsTermination, "Основание прекращения деятельности").Column("ACTIVITY_TERMINATION").NotNull();
            Property(x => x.DescriptionTermination, "Примечание прекращения деятельности").Column("DESCRIPTION_TERM").Length(500);
            Property(x => x.DateTermination, "Дата прекращения деятельности").Column("DATE_TERMINATION");
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
