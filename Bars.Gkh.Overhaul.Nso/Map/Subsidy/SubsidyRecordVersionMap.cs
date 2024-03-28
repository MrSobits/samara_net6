/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class SubsidyRecordVersionMap : BaseEntityMap<SubsidyRecordVersion>
///     {
///         public SubsidyRecordVersionMap()
///             : base("OVRHL_SUBSIDY_REC_VERSION")
///         {
///             Map(x => x.BudgetCr, "BUDGET_CR", true, 0);
///             Map(x => x.CorrectionFinance, "CORRECTION_FINANCES", true, 0);
///             Map(x => x.BalanceAfterCr, "BALANCE_AFTER_CR", true, 0);
/// 
///             References(x => x.SubsidyRecord, "SUBSIDY_REC_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Version, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.SubsidyRecordVersion"</summary>
    public class SubsidyRecordVersionMap : BaseEntityMap<SubsidyRecordVersion>
    {
        
        public SubsidyRecordVersionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.SubsidyRecordVersion", "OVRHL_SUBSIDY_REC_VERSION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Version, "Version").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.SubsidyRecord, "SubsidyRecord").Column("SUBSIDY_REC_ID").NotNull().Fetch();
            Property(x => x.BudgetCr, "BudgetCr").Column("BUDGET_CR").NotNull();
            Property(x => x.CorrectionFinance, "CorrectionFinance").Column("CORRECTION_FINANCES").NotNull();
            Property(x => x.BalanceAfterCr, "BalanceAfterCr").Column("BALANCE_AFTER_CR").NotNull();
        }
    }
}
