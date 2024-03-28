/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Притензия к управляющей организации"
///     /// </summary>
///     public class ManagingOrgClaimMap : BaseGkhEntityMap<ManagingOrgClaim>
///     {
///         public ManagingOrgClaimMap() : base("GKH_MAN_ORG_CLAIM")
///         {
///             Map(x => x.DateClaim, "DATE_CLAIM");
///             Map(x => x.AmountClaim, "AMOUNT");
///             Map(x => x.ContentClaim, "CONTENT").Length(500);
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
    
    
    /// <summary>Маппинг для "Претензия к управляющей организации"</summary>
    public class ManagingOrgClaimMap : BaseImportableEntityMap<ManagingOrgClaim>
    {
        
        public ManagingOrgClaimMap() : 
                base("Претензия к управляющей организации", "GKH_MAN_ORG_CLAIM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DateClaim, "Дата претензии").Column("DATE_CLAIM");
            Property(x => x.AmountClaim, "Сумма претензии").Column("AMOUNT");
            Property(x => x.ContentClaim, "Содержание претензии").Column("CONTENT").Length(500);
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MAN_ORG_ID").NotNull().Fetch();
        }
    }
}
