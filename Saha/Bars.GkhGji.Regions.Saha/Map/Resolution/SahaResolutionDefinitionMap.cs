/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Saha.Map
/// {
///     using Bars.GkhGji.Regions.Saha.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Определение постановления для САХИ"
///     /// </summary>
///     public class SahaResolutionDefinitionMap : SubclassMap<SahaResolutionDefinition>
///     {
///         public SahaResolutionDefinitionMap()
///         {
///             Table("SAHA_GJI_RESOLUTIONDEF");
///             KeyColumn("ID");
/// 
///             Map(x => x.TimeEnd, "TIME_END");
///             Map(x => x.TimeStart, "TIME_START");
/// 
///             References(x => x.FileDescription, "FILE_DESC_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Saha.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Saha.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Saha.Entities.SahaResolutionDefinition"</summary>
    public class SahaResolutionDefinitionMap : JoinedSubClassMap<SahaResolutionDefinition>
    {
        
        public SahaResolutionDefinitionMap() : 
                base("Bars.GkhGji.Regions.Saha.Entities.SahaResolutionDefinition", "SAHA_GJI_RESOLUTIONDEF")
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
