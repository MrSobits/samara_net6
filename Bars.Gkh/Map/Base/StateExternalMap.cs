/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// мап для StateExternal
///     /// </summary>
///     public class StateExternalMap : BaseGkhEntityMap<StateExternal>
///     {
///         public StateExternalMap() : base("CONVERTER_STATE_EXTERNAL")
///         {
///             Map(x => x.StateId, "STATE_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Таблица для конвертации связь страого external_id с статусом в новой"</summary>
    public class StateExternalMap : BaseImportableEntityMap<StateExternal>
    {
        
        public StateExternalMap() : 
                base("Таблица для конвертации связь страого external_id с статусом в новой", "CONVERTER_STATE_EXTERNAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.StateId, "Id статуса в новой").Column("STATE_ID");
        }
    }
}
