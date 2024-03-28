/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.ManOrg
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities.ManOrg;
/// 
///     public class ManagingOrgRegistryMap: BaseImportableEntityMap<ManagingOrgRegistry>
///     {
///         public ManagingOrgRegistryMap()
///             : base("GKH_MAN_ORG_REGISTRY")
///         {
///             Map(x => x.EgrulDate, "EGRUL_DATE");
///             Map(x => x.InfoDate, "INFO_DATE");
///             Map(x => x.InfoType, "INFO_TYPE");
///             Map(x => x.RegNumber, "REG_NUM");
/// 
///             References(x => x.Doc, "FILE_ID").Fetch.Join();
///             References(x => x.ManagingOrganization, "MAN_ORG_ID").Not.Nullable().Fetch.Join();
/// 
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.ManOrg
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.ManOrg;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.ManOrg.ManagingOrgRegistry"</summary>
    public class ManagingOrgRegistryMap : BaseImportableEntityMap<ManagingOrgRegistry>
    {
        
        public ManagingOrgRegistryMap() : 
                base("Bars.Gkh.Entities.ManOrg.ManagingOrgRegistry", "GKH_MAN_ORG_REGISTRY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.EgrulDate, "Дата внесения записи в ЕГРЮЛ").Column("EGRUL_DATE");
            Property(x => x.InfoDate, "Дата предоставления сведений").Column("INFO_DATE");
            Property(x => x.InfoType, "Тип сведений").Column("INFO_TYPE");
            Property(x => x.RegNumber, "Регистрационный номер").Column("REG_NUM");
            Reference(x => x.Doc, "Документ").Column("FILE_ID").Fetch();
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MAN_ORG_ID").NotNull().Fetch();
        }
    }
}
