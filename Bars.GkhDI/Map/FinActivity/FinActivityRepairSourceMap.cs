/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Объем привлеченных средств на ремонт и благоустройство финансовой деятельности"
///     /// </summary>
///     public class FinActivityRepairSourceMap : BaseGkhEntityMap<FinActivityRepairSource>
///     {
///         public FinActivityRepairSourceMap(): base("DI_DISINFO_FIN_REPAIR_SRC")
///         {
///             Map(x => x.TypeSourceFundsDi, "TYPE_SOURCE_FUNDS").Not.Nullable().CustomType<TypeSourceFundsDi>();
///             Map(x => x.Sum, "WORK_REPAIR");
/// 
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivityRepairSource"</summary>
    public class FinActivityRepairSourceMap : BaseImportableEntityMap<FinActivityRepairSource>
    {
        
        public FinActivityRepairSourceMap() : 
                base("Bars.GkhDi.Entities.FinActivityRepairSource", "DI_DISINFO_FIN_REPAIR_SRC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeSourceFundsDi, "TypeSourceFundsDi").Column("TYPE_SOURCE_FUNDS").NotNull();
            Property(x => x.Sum, "Sum").Column("WORK_REPAIR");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
        }
    }
}
