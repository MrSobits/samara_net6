namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дома в Протоколе МЖК"</summary>
    public class ProtocolRSORealityObjectMap : BaseEntityMap<ProtocolRSORealityObject>
    {
        
        public ProtocolRSORealityObjectMap() : 
                base("Дома в Протоколе РСО", "GJI_PROTOCOLRSO_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ProtocolRSO, "Протокол РСО").Column("PROTOCOLRSO_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
