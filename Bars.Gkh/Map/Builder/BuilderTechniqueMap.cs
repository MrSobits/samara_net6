/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Техника подрядчиков"
///     /// </summary>
///     public class BuilderTechniqueMap : BaseGkhEntityMap<BuilderTechnique>
///     {
///         public BuilderTechniqueMap()
///             : base("GKH_BUILDER_TECHNIQUE")
///         {
///             Map(x => x.Name, "Name").Length(300);
/// 
///             References(x => x.File, "FILE_ID").Fetch.Join();
///             References(x => x.Builder, "BUILDER_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Техника, Инструменты подрядчика"</summary>
    public class BuilderTechniqueMap : BaseImportableEntityMap<BuilderTechnique>
    {
        
        public BuilderTechniqueMap() : 
                base("Техника, Инструменты подрядчика", "GKH_BUILDER_TECHNIQUE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
        }
    }
}
