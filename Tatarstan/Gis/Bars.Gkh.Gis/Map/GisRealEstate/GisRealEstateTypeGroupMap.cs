/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.GisRealEstate
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstate.GisRealEstateType;
/// 
///     public class GisRealEstateTypeGroupMap : BaseEntityMap<GisRealEstateTypeGroup>
///     {
///         public GisRealEstateTypeGroupMap()
///             : base("GIS_REAL_EST_TYPE_GROUP")
///         {
///             Map(x => x.Name, "NAME", true, 300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.RealEstate.GisRealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType.GisRealEstateTypeGroup"</summary>
    public class GisRealEstateTypeGroupMap : BaseEntityMap<GisRealEstateTypeGroup>
    {
        
        public GisRealEstateTypeGroupMap() : 
                base("Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType.GisRealEstateTypeGroup", "GIS_REAL_EST_TYPE_GROUP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(300).NotNull();
        }
    }
}
