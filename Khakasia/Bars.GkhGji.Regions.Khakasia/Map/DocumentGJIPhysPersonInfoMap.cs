namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.DocumentGJIPhysPersonInfo"</summary>
    public class DocumentGJIPhysPersonInfoMap : BaseEntityMap<DocumentGJIPhysPersonInfo>
    {
        
        public DocumentGJIPhysPersonInfoMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.DocumentGJIPhysPersonInfo", "GJI_KHAKASIA_DOCGJI_PERSINFO")
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
