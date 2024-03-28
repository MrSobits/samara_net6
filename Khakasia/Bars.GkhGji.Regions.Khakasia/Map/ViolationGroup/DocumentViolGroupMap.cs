namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.DocumentViolGroup"</summary>
    public class DocumentViolGroupMap : BaseEntityMap<DocumentViolGroup>
    {
        
        public DocumentViolGroupMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.DocumentViolGroup", "GJI_DOC_VIOLGROUP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID");
            Reference(x => x.Document, "Document").Column("DOCUMENT_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            Property(x => x.Action, "Action").Column("ACTION").Length(500);
            Property(x => x.DatePlanRemoval, "DatePlanRemoval").Column("DATE_PLAN_REMOVAL");
            Property(x => x.DateFactRemoval, "DateFactRemoval").Column("DATE_FACT_REMOVAL");
        }
    }
}
