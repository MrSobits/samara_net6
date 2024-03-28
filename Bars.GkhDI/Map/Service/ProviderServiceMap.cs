/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     public class ProviderServiceMap : BaseGkhEntityMap<ProviderService>
///     {
///         public ProviderServiceMap()
///             : base("DI_SERVICE_PROVIDER")
///         {
///             Map(x => x.DateStartContract, "DATE_START_CONTRACT");
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.IsActive, "IS_ACTIVE");
/// 
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Provider, "PROVIDER_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.ProviderService"</summary>
    public class ProviderServiceMap : BaseImportableEntityMap<ProviderService>
    {
        
        public ProviderServiceMap() : 
                base("Bars.GkhDi.Entities.ProviderService", "DI_SERVICE_PROVIDER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DateStartContract, "DateStartContract").Column("DATE_START_CONTRACT");
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            Property(x => x.IsActive, "IsActive").Column("IS_ACTIVE");
            Property(x => x.NumberContract, "NumberContract").Column("NUMBER_CONTRACT");
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
            Reference(x => x.Provider, "Provider").Column("PROVIDER_ID").NotNull().Fetch();
        }
    }
}
