/// <mapping-converter-backup>
/// using Bars.Gkh.Entities;
/// 
/// namespace Bars.Gkh.Map.Base
/// {
///     public class HeatSeasExternalMap : BaseGkhEntityMap<HeatSeasExternal>
///     {
///         public HeatSeasExternalMap()
///             : base("CONVERTER_HEATS_EXTERNAL")
///         {
///             Map(x => x.NewExternalId, "NEW_EXTERNAL_ID").Length(36);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Таблица для конвертации, связь строго отопительного сезона и схлопнутого"</summary>
    public class HeatSeasExternalMap : BaseImportableEntityMap<HeatSeasExternal>
    {
        
        public HeatSeasExternalMap() : 
                base("Таблица для конвертации, связь строго отопительного сезона и схлопнутого", "CONVERTER_HEATS_EXTERNAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.NewExternalId, "новый идентификатор отопительного сезона").Column("NEW_EXTERNAL_ID").Length(36);
        }
    }
}
