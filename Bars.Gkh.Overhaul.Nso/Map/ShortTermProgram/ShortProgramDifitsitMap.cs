/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class ShortProgramDifitsitMap : BaseEntityMap<ShortProgramDifitsit>
///     {
///         public ShortProgramDifitsitMap()
///             : base("OVRHL_SHORT_PROG_DIFITSIT")
///         {
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.Difitsit, "DIFITSIT", true, 0);
///             Map(x => x.BudgetRegionShare, "REGION_BUDGET_SHARE", true, 0);
/// 
///             References(x => x.Municipality, "MUNICIPALITY_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Version, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.ShortProgramDifitsit"</summary>
    public class ShortProgramDifitsitMap : BaseEntityMap<ShortProgramDifitsit>
    {
        
        public ShortProgramDifitsitMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.ShortProgramDifitsit", "OVRHL_SHORT_PROG_DIFITSIT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.Version, "Version").Column("VERSION_ID").NotNull().Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.Difitsit, "Difitsit").Column("DIFITSIT").NotNull();
            Property(x => x.BudgetRegionShare, "BudgetRegionShare").Column("REGION_BUDGET_SHARE").NotNull();
        }
    }
}
