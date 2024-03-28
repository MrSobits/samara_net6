/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Управление домами по категориям финансовой деятельности"
///     /// </summary>
///     public class FinActivityManagCategoryMap : BaseGkhEntityMap<FinActivityManagCategory>
///     {
///         public FinActivityManagCategoryMap(): base("DI_DISINFO_FINACT_CATEG")
///         {
///             Map(x => x.TypeCategoryHouseDi, "TYPE_CATEGORY_HOUSE").Not.Nullable().CustomType<TypeCategoryHouseDi>();
///             Map(x => x.IncomeManaging, "INCOME_MANAGING");
///             Map(x => x.IncomeUsingGeneralProperty, "INCOM_USE_GEN_PROPERTY");
///             Map(x => x.ExpenseManaging, "EXPENSE_MANAGING");
///             Map(x => x.ExactPopulation, "EXACT_POPULATION");
///             Map(x => x.DebtPopulationStart, "DEBT_POPULATION_START");
///             Map(x => x.DebtPopulationEnd, "DEBT_POPULATION_END");
/// 
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivityManagCategory"</summary>
    public class FinActivityManagCategoryMap : BaseImportableEntityMap<FinActivityManagCategory>
    {
        
        public FinActivityManagCategoryMap() : 
                base("Bars.GkhDi.Entities.FinActivityManagCategory", "DI_DISINFO_FINACT_CATEG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeCategoryHouseDi, "TypeCategoryHouseDi").Column("TYPE_CATEGORY_HOUSE").NotNull();
            Property(x => x.IncomeManaging, "IncomeManaging").Column("INCOME_MANAGING");
            Property(x => x.IncomeUsingGeneralProperty, "IncomeUsingGeneralProperty").Column("INCOM_USE_GEN_PROPERTY");
            Property(x => x.ExpenseManaging, "ExpenseManaging").Column("EXPENSE_MANAGING");
            Property(x => x.ExactPopulation, "ExactPopulation").Column("EXACT_POPULATION");
            Property(x => x.DebtPopulationStart, "DebtPopulationStart").Column("DEBT_POPULATION_START");
            Property(x => x.DebtPopulationEnd, "DebtPopulationEnd").Column("DEBT_POPULATION_END");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
        }
    }
}
