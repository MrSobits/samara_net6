/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Органы местного самоуправления"
///     /// </summary>
///     public class LocalGovernmentMap : BaseGkhEntityMap<LocalGovernment>
///     {
///         public LocalGovernmentMap() : base("GKH_LOCAL_GOVERNMENT")
///         {
///             Map(x => x.OrgStateRole, "ORG_STATE_ROLE").Not.Nullable().CustomType<OrgStateRole>();
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.Email, "EMAIL").Length(50);
///             Map(x => x.NameDepartamentGkh, "NAME_DEP_GKH").Length(300);
///             Map(x => x.OfficialSite, "OFFICIAL_SITE").Length(50);
///             Map(x => x.Phone, "PHONE").Length(50);
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
    
    
    /// <summary>Маппинг для "Органы местного самоуправления"</summary>
    public class LocalGovernmentMap : BaseImportableEntityMap<LocalGovernment>
    {
        
        public LocalGovernmentMap() : 
                base("Органы местного самоуправления", "GKH_LOCAL_GOVERNMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.OrgStateRole, "Статус").Column("ORG_STATE_ROLE").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.Email, "Email").Column("EMAIL").Length(50);
            Property(x => x.NameDepartamentGkh, "Наименование подразделения ответсвенного за ЖКХ").Column("NAME_DEP_GKH").Length(300);
            Property(x => x.OfficialSite, "Официальный сайт подразделения").Column("OFFICIAL_SITE").Length(50);
            Property(x => x.Phone, "Телефон").Column("PHONE").Length(50);
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
