/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дома деятельности ТСЖ ГЖИ"
///     /// </summary>
///     public class ActivityTsjRealObjMap : BaseGkhEntityMap<ActivityTsjRealObj>
///     {
///         public ActivityTsjRealObjMap() : base("GJI_ACT_TSJ_REAL_OBJ")
///         {
///             References(x => x.ActivityTsj, "ACTIVITY_TSJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дома деятельности ТСЖ"</summary>
    public class ActivityTsjRealObjMap : BaseEntityMap<ActivityTsjRealObj>
    {
        
        public ActivityTsjRealObjMap() : 
                base("Дома деятельности ТСЖ", "GJI_ACT_TSJ_REAL_OBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ActivityTsj, "Деятельность ТСЖ").Column("ACTIVITY_TSJ_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJ_ID").NotNull().Fetch();
        }
    }
}
