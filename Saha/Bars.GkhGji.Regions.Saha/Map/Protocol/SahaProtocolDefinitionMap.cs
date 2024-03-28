/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Saha.Map
/// {
///     using Bars.GkhGji.Regions.Saha.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Определение протокола для САХИ"
///     /// </summary>
///     public class SahaProtocolDefinitionMap : SubclassMap<SahaProtocolDefinition>
///     {
///         public SahaProtocolDefinitionMap()
///         {
///             Table("SAHA_GJI_PROTOCOLDEF");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Saha.Entities.SahaProtocolDefinition"</summary>
    public class SahaProtocolDefinitionMap : JoinedSubClassMap<SahaProtocolDefinition>
    {
        
        public SahaProtocolDefinitionMap() : 
                base("Bars.GkhGji.Regions.Saha.Entities.SahaProtocolDefinition", "SAHA_GJI_PROTOCOLDEF")
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
