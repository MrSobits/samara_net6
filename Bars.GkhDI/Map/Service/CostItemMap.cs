/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Entities;
/// 
///     public class CostItemMap : BaseGkhEntityMap<CostItem>
///     {
///         public CostItemMap()
///             : base("DI_COST_ITEM")
///         {
///             Map(x => x.Name, "NAME").Length(300);
/// 
///             Map(x => x.Cost, "COST");
///             Map(x => x.Count, "COUNT");
///             Map(x => x.Sum, "SUM");
/// 
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.CostItem"</summary>
    public class CostItemMap : BaseImportableEntityMap<CostItem>
    {
        
        public CostItemMap() : 
                base("Bars.GkhDi.Entities.CostItem", "DI_COST_ITEM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.Cost, "Cost").Column("COST");
            Property(x => x.Count, "Count").Column("COUNT");
            Property(x => x.Sum, "Sum").Column("SUM");
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
