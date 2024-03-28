namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.InspectionGjiViolWording"</summary>
    public class InspectionGjiViolWordingMap : BaseEntityMap<InspectionGjiViolWording>
    {
        
        public InspectionGjiViolWordingMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.InspectionGjiViolWording", "GJI_KHAKASIA_INSP_VIOL_WORD")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.InspectionViolation, "InspectionViolation").Column("INSPECTION_VIOL_ID").NotNull().Fetch();
            Property(x => x.Wording, "Wording").Column("WORDING").Length(250);
        }
    }
}
