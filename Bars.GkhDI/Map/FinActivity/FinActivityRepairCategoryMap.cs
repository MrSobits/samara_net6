/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Ремонт дома и благоустройство территории, средний срок обслуживания МКД финансовой деятельности"
///     /// </summary>
///     public class FinActivityRepairCategoryMap : BaseGkhEntityMap<FinActivityRepairCategory>
///     {
///         public FinActivityRepairCategoryMap(): base("DI_DISINFO_FIN_REPAIR_CAT")
///         {
///             Map(x => x.TypeCategoryHouseDi, "TYPE_CATEGORY_HOUSE").Not.Nullable().CustomType<TypeCategoryHouseDi>();
///             Map(x => x.WorkByRepair, "WORK_REPAIR");
///             Map(x => x.WorkByBeautification, "WORK_BEAUTIFICATION");
///             Map(x => x.PeriodService, "PERIOD_SERVICE");
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
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivityRepairCategory"</summary>
    public class FinActivityRepairCategoryMap : BaseImportableEntityMap<FinActivityRepairCategory>
    {
        
        public FinActivityRepairCategoryMap() : 
                base("Bars.GkhDi.Entities.FinActivityRepairCategory", "DI_DISINFO_FIN_REPAIR_CAT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeCategoryHouseDi, "TypeCategoryHouseDi").Column("TYPE_CATEGORY_HOUSE").NotNull();
            Property(x => x.WorkByRepair, "WorkByRepair").Column("WORK_REPAIR");
            Property(x => x.WorkByBeautification, "WorkByBeautification").Column("WORK_BEAUTIFICATION");
            Property(x => x.PeriodService, "PeriodService").Column("PERIOD_SERVICE");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
        }
    }
}
