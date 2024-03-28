/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.Gkh.Entities.Dicts;
/// 
///     public class ResettlementProgramSourceMap : BaseGkhEntityMap<ResettlementProgramSource>
///     {
///         public ResettlementProgramSourceMap() : base("GKH_DICT_RESETTLE_SOURCE")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300).Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Источник по программе переселения"</summary>
    public class ResettlementProgramSourceMap : BaseImportableEntityMap<ResettlementProgramSource>
    {
        
        public ResettlementProgramSourceMap() : 
                base("Источник по программе переселения", "GKH_DICT_RESETTLE_SOURCE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
        }
    }
}
