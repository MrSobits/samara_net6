/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Zabaykalye.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     using Bars.GkhGji.Regions.Zabaykalye.Entities;
/// 
///     public class DocumentViolGroupMap : BaseEntityMap<DocumentViolGroup>
///     {
///         public DocumentViolGroupMap()
///             : base("GJI_DOC_VIOLGROUP")
///         {
///             this.References(x => x.Document, "DOCUMENT_ID", ReferenceMapConfig.NotNull);
///             this.References(x => x.RealityObject, "RO_ID");
///             this.Map(x => x.Description, "DESCRIPTION", false, 500);
///             this.Map(x => x.Action, "ACTION", false, 500);
///             this.Map(x => x.DatePlanRemoval, "DATE_PLAN_REMOVAL");
///             this.Map(x => x.DateFactRemoval, "DATE_FACT_REMOVAL");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Zabaykalye.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Zabaykalye.Entities.DocumentViolGroup"</summary>
    public class DocumentViolGroupMap : BaseEntityMap<DocumentViolGroup>
    {
        
        public DocumentViolGroupMap() : 
                base("Bars.GkhGji.Regions.Zabaykalye.Entities.DocumentViolGroup", "GJI_DOC_VIOLGROUP")
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
