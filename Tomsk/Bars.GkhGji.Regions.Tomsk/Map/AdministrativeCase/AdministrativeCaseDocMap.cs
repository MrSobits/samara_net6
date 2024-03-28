/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
///     using Bars.GkhGji.Regions.Tomsk.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Документы административного дела ГЖИ"
///     /// </summary>
///     public class AdministrativeCaseDocMap : BaseEntityMap<AdministrativeCaseDoc>
///     {
///         public AdministrativeCaseDocMap()
///             : base("GJI_ADMINCASE_DOC")
///         {
///             Map(x => x.TypeAdminCaseDoc, "TYPE_ADMIN_DOC").Not.Nullable().CustomType<TypeAdminCaseDoc>();
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER").Length(100);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.NeedTerm, "NEED_TERM");
///             Map(x => x.RenewalTerm, "RENEWAL_TERM");
///             Map(x => x.DescriptionSet, "DESCRIPTION_SET").Length(2000);
/// 
///             References(x => x.AdministrativeCase, "ADMINCASE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.EntitiedInspector, "ENTITIED_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseDoc"</summary>
    public class AdministrativeCaseDocMap : BaseEntityMap<AdministrativeCaseDoc>
    {
        
        public AdministrativeCaseDocMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseDoc", "GJI_ADMINCASE_DOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeAdminCaseDoc, "TypeAdminCaseDoc").Column("TYPE_ADMIN_DOC").NotNull();
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOCUMENT_NUMBER").Length(100);
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM");
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.NeedTerm, "NeedTerm").Column("NEED_TERM");
            Property(x => x.RenewalTerm, "RenewalTerm").Column("RENEWAL_TERM");
            Property(x => x.DescriptionSet, "DescriptionSet").Column("DESCRIPTION_SET").Length(2000);
            Reference(x => x.AdministrativeCase, "AdministrativeCase").Column("ADMINCASE_ID").NotNull().Fetch();
            Reference(x => x.EntitiedInspector, "EntitiedInspector").Column("ENTITIED_ID").Fetch();
        }
    }
}
