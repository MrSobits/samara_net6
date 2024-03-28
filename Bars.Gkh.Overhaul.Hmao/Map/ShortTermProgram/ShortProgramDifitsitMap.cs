/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class ShortProgramDifitsitMap : BaseImportableEntityMap<ShortProgramDifitsit>
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

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Дифицит по МО для года в краткосрочной программе КР"</summary>
    public class ShortProgramDifitsitMap : BaseImportableEntityMap<ShortProgramDifitsit>
    {
        
        public ShortProgramDifitsitMap() : 
                base("Дифицит по МО для года в краткосрочной программе КР", "OVRHL_SHORT_PROG_DIFITSIT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.Version, "Версия ДПКР").Column("VERSION_ID").NotNull().Fetch();
            Property(x => x.Year, "Год").Column("YEAR").NotNull();
            Property(x => x.Difitsit, "Дифицит в этом году (руб)").Column("DIFITSIT").NotNull();
            Property(x => x.BudgetRegionShare, "Доля средств из Бюджета региона (%)").Column("REGION_BUDGET_SHARE").NotNull();
        }
    }
}
