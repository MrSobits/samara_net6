
namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Протоколы собственников помещений МКД"</summary>
    public class PropertyOwnerProtocolInspectorMap : BaseImportableEntityMap<PropertyOwnerProtocolInspector>
    {
        
        public PropertyOwnerProtocolInspectorMap() : 
                base("Инспектор протокола ОСС", "OVRHL_PROP_OWN_PROTOCOLS_INSPECTOR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PropertyOwnerProtocols, "Протокол").Column("PROTOCOL_ID").Fetch();
            Reference(x => x.Inspector, "Решение").Column("INSPECTOR_ID").Fetch();
        }
    }
}
