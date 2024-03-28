/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Zabaykalye.Map
/// {
///     using Bars.GkhGji.Regions.Zabaykalye.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Определение постановления для Забайкалья"
///     /// </summary>
///     public class ZabaykalyeResolutionDefinitionMap : SubclassMap<ZabaykalyeResolutionDefinition>
///     {
///         public ZabaykalyeResolutionDefinitionMap()
///         {
///             Table("ZBKL_GJI_RESOLUTIONDEF");
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

namespace Bars.GkhGji.Regions.Zabaykalye.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Zabaykalye.Entities.ZabaykalyeResolutionDefinition"</summary>
    public class ZabaykalyeResolutionDefinitionMap : JoinedSubClassMap<ZabaykalyeResolutionDefinition>
    {
        
        public ZabaykalyeResolutionDefinitionMap() : 
                base("Bars.GkhGji.Regions.Zabaykalye.Entities.ZabaykalyeResolutionDefinition", "ZBKL_GJI_RESOLUTIONDEF")
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
