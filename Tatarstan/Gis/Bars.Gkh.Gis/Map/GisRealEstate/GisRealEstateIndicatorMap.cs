/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.GisRealEstate
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstate;
/// 
///     public class GisRealEstateIndicatorMap : PersistentObjectMap<RealEstateIndicator>
///     {
///         public GisRealEstateIndicatorMap()
///             : base("GIS_REAL_ESTATE_INDICATOR")
///         {
///             Map(x => x.Name, "NAME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.RealEstate
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.RealEstate;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.RealEstate.RealEstateIndicator"</summary>
    public class RealEstateIndicatorMap : PersistentObjectMap<RealEstateIndicator>
    {
        
        public RealEstateIndicatorMap() : 
                base("Bars.Gkh.Gis.Entities.RealEstate.RealEstateIndicator", "GIS_REAL_ESTATE_INDICATOR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(250);
        }
    }
}
