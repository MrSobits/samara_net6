/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "проверяемый дом"
///     /// </summary>
///     public class InspectionGjiRealityObjectMap : BaseGkhEntityMap<InspectionGjiRealityObject>
///     {
///         public InspectionGjiRealityObjectMap() : base("GJI_INSPECTION_ROBJECT")
///         {
///             Map(x => x.RoomNums, "ROOM_NUMS").Length(300);
/// 
///             References(x => x.Inspection, "INSPECTION_ID").Not.Nullable().LazyLoad();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Проверяемые дома в инспекционной проверки ГЖИ"</summary>
    public class InspectionGjiRealityObjectMap : BaseEntityMap<InspectionGjiRealityObject>
    {
        
        public InspectionGjiRealityObjectMap() : 
                base("Проверяемые дома в инспекционной проверки ГЖИ", "GJI_INSPECTION_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.RoomNums, "Номера квартир").Column("ROOM_NUMS").Length(300);
            Reference(x => x.Inspection, "Проверка ГЖИ").Column("INSPECTION_ID").NotNull();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull();
            this.Property(x => x.ErpGuid, nameof(InspectionGjiRealityObject.ErpGuid)).Column("ERP_GUID");
            Property(x => x.ErknmGuid, "Гуид ЕРКНМ").Column("ERKNM_GUID").Length(36);
        }
    }
}
