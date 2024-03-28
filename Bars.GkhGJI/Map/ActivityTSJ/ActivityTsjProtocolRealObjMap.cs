/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дома деятельности ТСЖ ГЖИ"
///     /// </summary>
///     public class ActivityTsjProtocolRealObjMap : BaseGkhEntityMap<ActivityTsjProtocolRealObj>
///     {
///         public ActivityTsjProtocolRealObjMap() : base("GJI_PROT_REAL_OBJ")
///         {
///             References(x => x.ActivityTsjProtocol, "PROTOCOL_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дома протокола деятельности ТСЖ"</summary>
    public class ActivityTsjProtocolRealObjMap : BaseEntityMap<ActivityTsjProtocolRealObj>
    {
        
        public ActivityTsjProtocolRealObjMap() : 
                base("Дома протокола деятельности ТСЖ", "GJI_PROT_REAL_OBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ActivityTsjProtocol, "Протокол деятельности ТСЖ").Column("PROTOCOL_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJ_ID").NotNull().Fetch();
        }
    }
}
