/// <mapping-converter-backup>
/// using Bars.B4.DataAccess.ByCode;
/// using Bars.B4.DataAccess.UserTypes;
/// using Bars.GkhGji.Regions.Tatarstan.Entities;
/// 
/// namespace Bars.GkhGji.Regions.Tatarstan.Map
/// {
///     public class GisGmpPatternMap: BaseEntityMap<GisGmpPattern>
///     {
///         public GisGmpPatternMap()
///             : base("GJI_TAT_GIS_GMP_PATTERN")
///         {
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.PatternCode, "PATTERN_CODE", true, 200);
/// 
///             References(x => x.Municipality, "MUNICIPALITY_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tatarstan.Entities.GisGmpPattern"</summary>
    public class GisGmpPatternMap : BaseEntityMap<GisGmpPattern>
    {
        
        public GisGmpPatternMap() : 
                base("Bars.GkhGji.Regions.Tatarstan.Entities.GisGmpPattern", "GJI_TAT_GIS_GMP_PATTERN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateStart, "DateStart").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.PatternCode, "PatternCode").Column("PATTERN_CODE").Length(200).NotNull();
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
