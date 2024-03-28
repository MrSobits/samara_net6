/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Hcs
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities.Hcs;
/// 
///     public class HouseInfoOverviewMap : BaseImportableEntityMap<HouseInfoOverview>
///     {
///         public HouseInfoOverviewMap()
///             : base("HCS_HOUSE_INFO_OVERVIEW")
///         {
///             Map(x => x.IndividualAccountsCount, "INDIV_ACCOUNTS_COUNT");
///             Map(x => x.IndividualOwnerAccountsCount, "INDIV_OWNER_ACCOUNTS_COUNT");
///             Map(x => x.IndividualTenantAccountsCount, "INDIV_TENANT_ACCOUNTS_COUNT");
///             Map(x => x.LegalAccountsCount, "LEGAL_ACCOUNTS_COUNT");
///             Map(x => x.LegalOwnerAccountsCount, "LEGAL_OWNER_ACCOUNTS_COUNT");
///             Map(x => x.LegalTenantAccountsCount, "LEGAL_TENANT_ACCOUNTS_COUNT");
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Hcs
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Hcs;
    
    
    /// <summary>Маппинг для "Общие сведения по дому"</summary>
    public class HouseInfoOverviewMap : BaseImportableEntityMap<HouseInfoOverview>
    {
        
        public HouseInfoOverviewMap() : 
                base("Общие сведения по дому", "HCS_HOUSE_INFO_OVERVIEW")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Property(x => x.IndividualAccountsCount, "Общее количество лицевых счетов физических лиц").Column("INDIV_ACCOUNTS_COUNT");
            Property(x => x.IndividualTenantAccountsCount, "Количество лицевых счетов физических лиц-собственников").Column("INDIV_TENANT_ACCOUNTS_COUNT");
            Property(x => x.IndividualOwnerAccountsCount, "Количество лицевых счетов физических лиц-нанимателей").Column("INDIV_OWNER_ACCOUNTS_COUNT");
            Property(x => x.LegalAccountsCount, "Общее количество лицевых счетов юридических лиц").Column("LEGAL_ACCOUNTS_COUNT");
            Property(x => x.LegalTenantAccountsCount, "Количество лицевых счетов юридических лиц-собственников").Column("LEGAL_TENANT_ACCOUNTS_COUNT");
            Property(x => x.LegalOwnerAccountsCount, "Количество лицевых счетов юридических лиц-нанимателей").Column("LEGAL_OWNER_ACCOUNTS_COUNT");
        }
    }
}
