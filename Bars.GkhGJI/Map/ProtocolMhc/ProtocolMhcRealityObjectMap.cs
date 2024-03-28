/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дом протокола мжк"
///     /// </summary>
///     public class ProtocolMhcRealityObjectMap : BaseGkhEntityMap<ProtocolMhcRealityObject>
///     {
///         public ProtocolMhcRealityObjectMap()
///             : base("GJI_PROTOCOLMHC_ROBJECT")
///         {
///             References(x => x.ProtocolMhc, "PROTOCOLMHC_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дома в Протоколе МЖК"</summary>
    public class ProtocolMhcRealityObjectMap : BaseEntityMap<ProtocolMhcRealityObject>
    {
        
        public ProtocolMhcRealityObjectMap() : 
                base("Дома в Протоколе МЖК", "GJI_PROTOCOLMHC_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ProtocolMhc, "Протокол МЖК").Column("PROTOCOLMHC_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
