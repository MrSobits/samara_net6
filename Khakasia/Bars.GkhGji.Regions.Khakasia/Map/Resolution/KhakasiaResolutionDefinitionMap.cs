namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.KhakasiaResolutionDefinition"</summary>
    public class KhakasiaResolutionDefinitionMap : JoinedSubClassMap<KhakasiaResolutionDefinition>
    {
        
        public KhakasiaResolutionDefinitionMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.KhakasiaResolutionDefinition", "KHAKASIA_GJI_RESOLUTIONDEF")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TimeEnd, "TimeEnd").Column("TIME_END");
            Property(x => x.TimeStart, "TimeStart").Column("TIME_START");
            Reference(x => x.FileDescription, "FileDescription").Column("FILE_DESC_ID").Fetch();
        }
    }
}
