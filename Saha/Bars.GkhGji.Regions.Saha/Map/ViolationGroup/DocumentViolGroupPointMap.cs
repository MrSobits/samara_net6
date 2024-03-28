/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Saha.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     using Bars.GkhGji.Regions.Saha.Entities;
/// 
///     public class DocumentViolGroupPointMap : BaseEntityMap<DocumentViolGroupPoint>
///     {
///         public DocumentViolGroupPointMap()
///             : base("GJI_DOC_VIOLGROUP_POINT")
///         {
///             this.References(x => x.ViolGroup, "VIOLGROUP_ID");
///             this.References(x => x.ViolStage, "VIOLSTAGE_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Saha.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Saha.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Saha.Entities.DocumentViolGroupPoint"</summary>
    public class DocumentViolGroupPointMap : BaseEntityMap<DocumentViolGroupPoint>
    {
        
        public DocumentViolGroupPointMap() : 
                base("Bars.GkhGji.Regions.Saha.Entities.DocumentViolGroupPoint", "GJI_DOC_VIOLGROUP_POINT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ViolGroup, "ViolGroup").Column("VIOLGROUP_ID");
            Reference(x => x.ViolStage, "ViolStage").Column("VIOLSTAGE_ID");
        }
    }
}
