namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.DocumentViolGroupPoint"</summary>
    public class DocumentViolGroupPointMap : BaseEntityMap<DocumentViolGroupPoint>
    {
        
        public DocumentViolGroupPointMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.DocumentViolGroupPoint", "GJI_DOC_VIOLGROUP_POINT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ViolGroup, "ViolGroup").Column("VIOLGROUP_ID");
            Reference(x => x.ViolStage, "ViolStage").Column("VIOLSTAGE_ID");
        }
    }
}
