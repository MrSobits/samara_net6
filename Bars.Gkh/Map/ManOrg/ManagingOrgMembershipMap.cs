/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Членство управляющей организации"
///     /// </summary>
///     public class ManagingOrgMembershipMap : BaseGkhEntityMap<ManagingOrgMembership>
///     {
///         public ManagingOrgMembershipMap() : base("GKH_MAN_ORG_MEMBERSHIP")
///         {
///             Map(x => x.Address, "ADDRESS").Length(1000);
///             Map(x => x.Name, "NAME").Length(250);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.OfficialSite, "OFFICIAL_SITE").Length(250);
/// 
///             References(x => x.ManagingOrganization, "MAN_ORG_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Членство в объединениях"</summary>
    public class ManagingOrgMembershipMap : BaseImportableEntityMap<ManagingOrgMembership>
    {
        
        public ManagingOrgMembershipMap() : 
                base("Членство в объединениях", "GKH_MAN_ORG_MEMBERSHIP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Address, "Адрес").Column("ADDRESS").Length(1000);
            Property(x => x.Name, "Наименование").Column("NAME").Length(250);
            Property(x => x.DocumentNum, "Номер свидетельства о членстве").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            Property(x => x.OfficialSite, "Официальный сайт").Column("OFFICIAL_SITE").Length(250);
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MAN_ORG_ID").NotNull().Fetch();
        }
    }
}
