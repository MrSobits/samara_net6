/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дом протокола МВД"
///     /// </summary>
///     public class ProtocolMvdRealityObjectMap : BaseGkhEntityMap<ProtocolMvdRealityObject>
///     {
///         public ProtocolMvdRealityObjectMap()
///             : base("GJI_PROT_MVD_ROBJECT")
///         {
///             References(x => x.ProtocolMvd, "PROT_MVD_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дома в протоколе МВД"</summary>
    public class ProtocolMvdRealityObjectMap : BaseEntityMap<ProtocolMvdRealityObject>
    {
        
        public ProtocolMvdRealityObjectMap() : 
                base("Дома в протоколе МВД", "GJI_PROT_MVD_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ProtocolMvd, "Протокол МВД").Column("PROT_MVD_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
