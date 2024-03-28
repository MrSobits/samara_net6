/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Приложение Административного дела ГЖИ"
///     /// </summary>
///     public class AdministrativeCaseAnnexMap : BaseEntityMap<AdministrativeCaseAnnex>
///     {
///         public AdministrativeCaseAnnexMap()
///             : base("GJI_ADMINCASE_ANNEX")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
/// 
///             References(x => x.AdministrativeCase, "ADMINCASE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseAnnex"</summary>
    public class AdministrativeCaseAnnexMap : BaseEntityMap<AdministrativeCaseAnnex>
    {
        
        public AdministrativeCaseAnnexMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseAnnex", "GJI_ADMINCASE_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(2000);
            Reference(x => x.AdministrativeCase, "AdministrativeCase").Column("ADMINCASE_ID").NotNull().Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
