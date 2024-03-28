/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class ShortProgramRecordMap : BaseEntityMap<ShortProgramRecord>
///     {
///         public ShortProgramRecordMap()
///             : base("OVRHL_SHORT_PROG_REC")
///         {
///             Map(x => x.OwnerSumForCr, "OWNER_SUM_CR", true, 0);
///             Map(x => x.BudgetRegion, "BUDGET_REGION", true, 0);
///             Map(x => x.BudgetMunicipality, "BUDGET_MU", true, 0);
///             Map(x => x.BudgetFcr, "BUDGET_FSR", true, 0);
///             Map(x => x.BudgetOtherSource, "BUDGET_OTHER_SRC", true, 0);
///             Map(x => x.Difitsit, "DIFITSIT", true, 0);
///             Map(x => x.Year, "YEAR", true, 0);
/// 
///             References(x => x.Stage2, "STAGE2_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.ShortProgramRecord"</summary>
    public class ShortProgramRecordMap : BaseEntityMap<ShortProgramRecord>
    {
        
        public ShortProgramRecordMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.ShortProgramRecord", "OVRHL_SHORT_PROG_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").Fetch();
            Reference(x => x.Stage2, "Stage2").Column("STAGE2_ID").Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.OwnerSumForCr, "OwnerSumForCr").Column("OWNER_SUM_CR").NotNull();
            Property(x => x.BudgetFcr, "BudgetFcr").Column("BUDGET_FSR").NotNull();
            Property(x => x.BudgetOtherSource, "BudgetOtherSource").Column("BUDGET_OTHER_SRC").NotNull();
            Property(x => x.BudgetRegion, "BudgetRegion").Column("BUDGET_REGION").NotNull();
            Property(x => x.BudgetMunicipality, "BudgetMunicipality").Column("BUDGET_MU").NotNull();
            Property(x => x.Difitsit, "Difitsit").Column("DIFITSIT").NotNull();
        }
    }
}
