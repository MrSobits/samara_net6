/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.GisRealEstate
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstate.GisRealEstateType;
/// 
///     public class GisRealEstateTypeMap : BaseEntityMap<GisRealEstateType>
///     {
///         public GisRealEstateTypeMap()
///             : base("GIS_REAL_ESTATE_TYPE")
///         {
///             Map(x => x.Name, "NAME", true, 300);
///             References(x => x.Group, "REAL_EST_TYPE_GROUP_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.RealEstate.GisRealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType.GisRealEstateType"</summary>
    public class GisRealEstateTypeMap : BaseEntityMap<GisRealEstateType>
    {
        
        public GisRealEstateTypeMap() : 
                base("Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType.GisRealEstateType", "GIS_REAL_ESTATE_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(300).NotNull();
            Reference(x => x.Group, "Group").Column("REAL_EST_TYPE_GROUP_ID").Fetch();
        }
    }
}
