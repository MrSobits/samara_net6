/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tula.Map
/// {
///     using Bars.GkhGji.Regions.Tula.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Определение постановления для Тулы"
///     /// </summary>
///     public class TulaResolutionDefinitionMap : SubclassMap<TulaResolutionDefinition>
///     {
///         public TulaResolutionDefinitionMap()
///         {
///             Table("TULA_GJI_RESOLUTIONDEF");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tula.Entities.TulaResolutionDefinition"</summary>
    public class TulaResolutionDefinitionMap : JoinedSubClassMap<TulaResolutionDefinition>
    {
        
        public TulaResolutionDefinitionMap() : 
                base("Bars.GkhGji.Regions.Tula.Entities.TulaResolutionDefinition", "TULA_GJI_RESOLUTIONDEF")
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
