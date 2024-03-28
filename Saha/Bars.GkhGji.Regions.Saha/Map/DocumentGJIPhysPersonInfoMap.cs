/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Saha.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Saha.Entities;
/// 
///     public class DocumentGJIPhysPersonInfoMap: BaseEntityMap<DocumentGJIPhysPersonInfo>
///     {
///         public DocumentGJIPhysPersonInfoMap()
///             : base("GJI_SAHA_DOCGJI_PERSINFO")
///         {
///             Map(x => x.PhysPersonAddress, "ADDRESS");
///             Map(x => x.PhysPersonJob, "JOB");
///             Map(x => x.PhysPersonPosition, "POSITION");
///             Map(x => x.PhysPersonBirthdayAndPlace, "BIRTHDAY_AND_PLACE");
///             Map(x => x.PhysPersonDocument, "IDENTITY_DOCUMENT");
///             Map(x => x.PhysPersonSalary, "SALARY");
///             Map(x => x.PhysPersonMaritalStatus, "MARITAL_STATUS");
///             
///             References(x => x.Document, "DOCUMENT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Saha.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Saha.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Saha.Entities.DocumentGJIPhysPersonInfo"</summary>
    public class DocumentGJIPhysPersonInfoMap : BaseEntityMap<DocumentGJIPhysPersonInfo>
    {
        
        public DocumentGJIPhysPersonInfoMap() : 
                base("Bars.GkhGji.Regions.Saha.Entities.DocumentGJIPhysPersonInfo", "GJI_SAHA_DOCGJI_PERSINFO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Document, "Document").Column("DOCUMENT_ID").NotNull().Fetch();
            Property(x => x.PhysPersonAddress, "PhysPersonAddress").Column("ADDRESS").Length(250);
            Property(x => x.PhysPersonJob, "PhysPersonJob").Column("JOB").Length(250);
            Property(x => x.PhysPersonPosition, "PhysPersonPosition").Column("POSITION").Length(250);
            Property(x => x.PhysPersonBirthdayAndPlace, "PhysPersonBirthdayAndPlace").Column("BIRTHDAY_AND_PLACE").Length(250);
            Property(x => x.PhysPersonDocument, "PhysPersonDocument").Column("IDENTITY_DOCUMENT").Length(250);
            Property(x => x.PhysPersonSalary, "PhysPersonSalary").Column("SALARY").Length(250);
            Property(x => x.PhysPersonMaritalStatus, "PhysPersonMaritalStatus").Column("MARITAL_STATUS").Length(250);
        }
    }
}
