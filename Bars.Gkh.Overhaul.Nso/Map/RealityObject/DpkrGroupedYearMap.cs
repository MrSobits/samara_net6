/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.RealityObject
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class DpkrGroupedYearMap : BaseEntityMap<DpkrGroupedYear>
///     {
///         public DpkrGroupedYearMap()
///             : base("OVRHL_DPKR_GROUPYEAR")
///         {
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.Year, "YEAR", true, 0);
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.DpkrGroupedYear"</summary>
    public class DpkrGroupedYearMap : BaseEntityMap<DpkrGroupedYear>
    {
        
        public DpkrGroupedYearMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.DpkrGroupedYear", "OVRHL_DPKR_GROUPYEAR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
        }
    }
}
