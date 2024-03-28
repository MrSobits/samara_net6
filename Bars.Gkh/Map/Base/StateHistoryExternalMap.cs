/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// мап для StateHistoryExternal
///     /// </summary>
///     public class StateHistoryExternalMap : BaseGkhEntityMap<StateHistoryExternal>
///     {
///         public StateHistoryExternalMap()
///             : base("CONVERTER_STHIST_EXTERNAL")
///         {
///             Map(x => x.StateHistoryId, "STATE_HISTORY_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Таблица для конвертации связь страого external_id с историей статуса в новой"</summary>
    public class StateHistoryExternalMap : BaseImportableEntityMap<StateHistoryExternal>
    {
        
        public StateHistoryExternalMap() : 
                base("Таблица для конвертации связь страого external_id с историей статуса в новой", "CONVERTER_STHIST_EXTERNAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.StateHistoryId, "Id статуса в новой").Column("STATE_HISTORY_ID");
        }
    }
}
