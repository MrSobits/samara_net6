/// <mapping-converter-backup>
/// namespace Bars.Gkh.Entities
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Инспектор
///     /// </summary>
///     public class InspectorSubscriptionMap : BaseImportableEntityMap<InspectorSubscription>
///     {
///         public InspectorSubscriptionMap()
///             : base("GKH_DICT_INSP_SUBSCRIP")
///         {
///             References(x => x.Inspector, "INSP_ID").Fetch.Join();
///             References(x => x.SignedInspector, "SIGNED_INSP_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Подписка инспекторов"</summary>
    public class InspectorSubscriptionMap : BaseImportableEntityMap<InspectorSubscription>
    {
        
        public InspectorSubscriptionMap() : 
                base("Подписка инспекторов", "GKH_DICT_INSP_SUBSCRIP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSP_ID").Fetch();
            Reference(x => x.SignedInspector, "Инсектор, который подписан на {Inspector}").Column("SIGNED_INSP_ID").Fetch();
        }
    }
}
