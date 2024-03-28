/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
///     using Enums;
/// 
///     public class RequirementMap : BaseEntityMap<Requirement>
///     {
///         public RequirementMap()
///             : base("GJI_REQUIREMENT")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER").Length(50);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.Destination, "DESTINATION").Length(500);
///             Map(x => x.TypeRequirement, "TYPE_REQUIREMENT").Not.Nullable().CustomType<TypeRequirement>();
///             Map(x => x.MaterialSubmitDate, "MATERIAL_SUB_DATE");
///             Map(x => x.AddMaterials, "ADD_MATERIALS").Length(2000);
///             Map(x => x.InspectionDate, "INSPECTION_DATE");
///             Map(x => x.InspectionHour, "INSPECTION_HOUR");
///             Map(x => x.InspectionMinute, "INSPECTION_MIN");
/// 
///             References(x => x.Document, "DOCUMENT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         } 
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.Requirement"</summary>
    public class RequirementMap : BaseEntityMap<Requirement>
    {
        
        public RequirementMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.Requirement", "GJI_REQUIREMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOCUMENT_NUMBER").Length(50);
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM");
            Property(x => x.Destination, "Destination").Column("DESTINATION").Length(500);
            Property(x => x.TypeRequirement, "TypeRequirement").Column("TYPE_REQUIREMENT").NotNull();
            Property(x => x.MaterialSubmitDate, "MaterialSubmitDate").Column("MATERIAL_SUB_DATE");
            Property(x => x.AddMaterials, "AddMaterials").Column("ADD_MATERIALS").Length(2000);
            Property(x => x.InspectionDate, "InspectionDate").Column("INSPECTION_DATE");
            Property(x => x.InspectionHour, "InspectionHour").Column("INSPECTION_HOUR");
            Property(x => x.InspectionMinute, "InspectionMinute").Column("INSPECTION_MIN");
            Reference(x => x.Document, "Document").Column("DOCUMENT_ID").NotNull().Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
