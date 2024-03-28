namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Отдела в проверке"</summary>
    public class InspectionGjiZonalInspectionMap : BaseEntityMap<InspectionGjiZonalInspection>
    {
        
        public InspectionGjiZonalInspectionMap() : 
                base("Инспектора в проверке", "GJI_INSPECTION_ZONAL_INSPECTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Inspection, "Проверка ГЖИ").Column("INSPECTION_ID").NotNull();
            Reference(x => x.ZonalInspection, "Отдел").Column("ZONAL_INSPECTION_ID").NotNull().Fetch();
        }
    }
}
