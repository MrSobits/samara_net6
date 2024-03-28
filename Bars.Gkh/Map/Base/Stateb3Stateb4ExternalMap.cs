/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// мап для Stateb3Stateb4External
///     /// </summary>
///     public class Stateb3Stateb4ExternalMap : BaseEntityMap<Stateb3Stateb4External>
///     {
///         public Stateb3Stateb4ExternalMap()
///             : base("CONVERTER_STB3B4_EXTERNAL")
///         {
///             Map(x => x.StateB3Id, "STATE_B3");
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Таблица для конвертации связь статусов б3 с б4"</summary>
    public class Stateb3Stateb4ExternalMap : BaseEntityMap<Stateb3Stateb4External>
    {
        
        public Stateb3Stateb4ExternalMap() : 
                base("Таблица для конвертации связь статусов б3 с б4", "CONVERTER_STB3B4_EXTERNAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.StateB3Id, "Id статуса в б3").Column("STATE_B3");
            Reference(x => x.State, "Id статуса в б4").Column("STATE_ID").Fetch();
        }
    }
}
