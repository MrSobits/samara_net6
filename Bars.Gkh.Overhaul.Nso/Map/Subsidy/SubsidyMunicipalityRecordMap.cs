/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class SubsidyMunicipalityRecordMap : BaseEntityMap<SubsidyMunicipalityRecord>
///     {
///         public SubsidyMunicipalityRecordMap()
///             : base("OVRHL_SUBSIDY_MU_REC")
///         {
///             Map(x => x.SubsidyYear, "SUBCIDY_YEAR", true, 0);
///             Map(x => x.BudgetFund, "BUDGET_FUND", true, 0);
///             Map(x => x.BudgetRegion, "BUDGET_REGION", true, 0);
///             Map(x => x.BudgetMunicipality, "BUDGET_MU", true, 0);
///             Map(x => x.OtherSource, "OTHER_SRC", true, 0);
///             Map(x => x.CalculatedCollection, "CALC_COLLECTION", true, 0);
///             Map(x => x.OwnersLimit, "OWNERS_LIMIT", true, 0);
///             Map(x => x.EstablishedTarif, "ESTABLISHED_TARIF", true, 0);
///             Map(x => x.ShareBudgetFund, "SHARE_BUDGET_FUND", true, 0);
///             Map(x => x.ShareBudgetRegion, "SHARE_BUDGET_REGION", true, 0);
///             Map(x => x.ShareBudgetMunicipality, "SHARE_BUDGET_MU", true, 0);
///             Map(x => x.ShareOtherSource, "SHARE_OTHER_SRC", true, 0);
///             Map(x => x.ShareOwnerFounds, "SHARE_OWNER_FOUNDS", true, 0);
///             Map(x => x.DeficitAfter, "DEFICIT_AFTER", true, 0);
///             Map(x => x.StartRecommendedTarif, "START_RECOM_TARIF", true, 0);
/// 
///             References(x => x.SubsidyMunicipality, "SUBSIDY_MU_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.SubsidyMunicipalityRecord"</summary>
    public class SubsidyMunicipalityRecordMap : BaseEntityMap<SubsidyMunicipalityRecord>
    {
        
        public SubsidyMunicipalityRecordMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.SubsidyMunicipalityRecord", "OVRHL_SUBSIDY_MU_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SubsidyMunicipality, "SubsidyMunicipality").Column("SUBSIDY_MU_ID").NotNull().Fetch();
            Property(x => x.SubsidyYear, "SubsidyYear").Column("SUBCIDY_YEAR").NotNull();
            Property(x => x.BudgetFund, "BudgetFund").Column("BUDGET_FUND").NotNull();
            Property(x => x.BudgetRegion, "BudgetRegion").Column("BUDGET_REGION").NotNull();
            Property(x => x.BudgetMunicipality, "BudgetMunicipality").Column("BUDGET_MU").NotNull();
            Property(x => x.OtherSource, "OtherSource").Column("OTHER_SRC").NotNull();
            Property(x => x.CalculatedCollection, "CalculatedCollection").Column("CALC_COLLECTION").NotNull();
            Property(x => x.OwnersLimit, "OwnersLimit").Column("OWNERS_LIMIT").NotNull();
            Property(x => x.ShareBudgetFund, "ShareBudgetFund").Column("SHARE_BUDGET_FUND").NotNull();
            Property(x => x.ShareBudgetRegion, "ShareBudgetRegion").Column("SHARE_BUDGET_REGION").NotNull();
            Property(x => x.ShareBudgetMunicipality, "ShareBudgetMunicipality").Column("SHARE_BUDGET_MU").NotNull();
            Property(x => x.ShareOtherSource, "ShareOtherSource").Column("SHARE_OTHER_SRC").NotNull();
            Property(x => x.ShareOwnerFounds, "ShareOwnerFounds").Column("SHARE_OWNER_FOUNDS").NotNull();
            Property(x => x.EstablishedTarif, "EstablishedTarif").Column("ESTABLISHED_TARIF").NotNull();
            Property(x => x.StartRecommendedTarif, "StartRecommendedTarif").Column("START_RECOM_TARIF").NotNull();
            Property(x => x.DeficitAfter, "DeficitAfter").Column("DEFICIT_AFTER").NotNull();
        }
    }
}
