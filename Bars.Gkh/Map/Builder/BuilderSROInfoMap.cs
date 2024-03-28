/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Сведения об участии в СРО"
///     /// </summary>
///     public class BuilderSroInfoMap : BaseGkhEntityMap<BuilderSroInfo>
///     {
///         public BuilderSroInfoMap()
///             : base("GKH_BUILDER_SROINFO")
///         {
///             References(x => x.Work, "WORK_ID").Fetch.Join();
///             References(x => x.DescriptionWork, "FILE_ID").Fetch.Join();
///             References(x => x.Builder, "BUILDER_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Сведения об участии в СРО"</summary>
    public class BuilderSroInfoMap : BaseImportableEntityMap<BuilderSroInfo>
    {
        
        public BuilderSroInfoMap() : 
                base("Сведения об участии в СРО", "GKH_BUILDER_SROINFO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Work, "Работа").Column("WORK_ID").Fetch();
            Reference(x => x.DescriptionWork, "Описание работ").Column("FILE_ID").Fetch();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
        }
    }
}
