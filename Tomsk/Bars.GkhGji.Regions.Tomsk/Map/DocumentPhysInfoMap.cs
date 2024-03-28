/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class DocumentPhysInfoMap : BaseEntityMap<DocumentPhysInfo>
///     {
///         public DocumentPhysInfoMap() : base("GJI_DOCUMENT_PHYS_INFO")
///         {
///             References(x => x.Document, "DOCUMENT_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.TypeGender, "TYPE_GENDER", true, (object) 0);
///             Map(x => x.PhysAddress, "PADDRESS", false, 2000);
///             Map(x => x.PhysJob, "PJOB", false, 2000);
///             Map(x => x.PhysPosition, "PPOSITION", false, 2000);
///             Map(x => x.PhysBirthdayAndPlace, "BIRTHDAY_AND_PLACE", false, 2000);
///             Map(x => x.PhysIdentityDoc, "PIDENTITY_DOC", false, 2000);
///             Map(x => x.PhysSalary, "PSALARY", false, 2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.DocumentPhysInfo"</summary>
    public class DocumentPhysInfoMap : BaseEntityMap<DocumentPhysInfo>
    {
        
        public DocumentPhysInfoMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.DocumentPhysInfo", "GJI_DOCUMENT_PHYS_INFO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Document, "Document").Column("DOCUMENT_ID").NotNull().Fetch();
            Property(x => x.TypeGender, "TypeGender").Column("TYPE_GENDER").DefaultValue(TypeGender.NotSet).NotNull();
            Property(x => x.PhysAddress, "PhysAddress").Column("PADDRESS").Length(2000);
            Property(x => x.PhysJob, "PhysJob").Column("PJOB").Length(2000);
            Property(x => x.PhysPosition, "PhysPosition").Column("PPOSITION").Length(2000);
            Property(x => x.PhysBirthdayAndPlace, "PhysBirthdayAndPlace").Column("BIRTHDAY_AND_PLACE").Length(2000);
            Property(x => x.PhysIdentityDoc, "PhysIdentityDoc").Column("PIDENTITY_DOC").Length(2000);
            Property(x => x.PhysSalary, "PhysSalary").Column("PSALARY").Length(2000);
        }
    }
}
