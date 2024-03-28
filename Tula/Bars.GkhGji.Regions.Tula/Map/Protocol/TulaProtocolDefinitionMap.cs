/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tula.Map
/// {
///     using Bars.GkhGji.Regions.Tula.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Определение протокола для Тулы"
///     /// </summary>
///     public class TulaProtocolDefinitionMap : SubclassMap<TulaProtocolDefinition>
///     {
///         public TulaProtocolDefinitionMap()
///         {
///             Table("TULA_GJI_PROTOCOLDEF");
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

namespace Bars.GkhGji.Regions.Tula.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tula.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tula.Entities.TulaProtocolDefinition"</summary>
    public class TulaProtocolDefinitionMap : JoinedSubClassMap<TulaProtocolDefinition>
    {
        
        public TulaProtocolDefinitionMap() : 
                base("Bars.GkhGji.Regions.Tula.Entities.TulaProtocolDefinition", "TULA_GJI_PROTOCOLDEF")
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
