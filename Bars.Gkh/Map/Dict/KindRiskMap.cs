/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Виды рисков
///     /// </summary>
///     public class KindRiskMap : BaseGkhEntityMap<KindRisk>
///     {
///         public KindRiskMap()
///             : base("GKH_DICT_KIND_RISK")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Виды рисков"</summary>
    public class KindRiskMap : BaseImportableEntityMap<KindRisk>
    {
        
        public KindRiskMap() : 
                base("Виды рисков", "GKH_DICT_KIND_RISK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(300);
        }
    }
}
