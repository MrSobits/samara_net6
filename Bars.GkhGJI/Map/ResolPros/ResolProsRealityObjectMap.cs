/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дом постановления прокуратуры ГЖИ"
///     /// </summary>
///     public class ResolProsRealityObjectMap : BaseGkhEntityMap<ResolProsRealityObject>
///     {
///         public ResolProsRealityObjectMap()
///             : base("GJI_RESOLPROS_ROBJECT")
///         {
///             References(x => x.ResolPros, "RESOLPROS_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дома в постановлении прокуратуры"</summary>
    public class ResolProsRealityObjectMap : BaseEntityMap<ResolProsRealityObject>
    {
        
        public ResolProsRealityObjectMap() : 
                base("Дома в постановлении прокуратуры", "GJI_RESOLPROS_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ResolPros, "Постановление прокуратуры").Column("RESOLPROS_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
