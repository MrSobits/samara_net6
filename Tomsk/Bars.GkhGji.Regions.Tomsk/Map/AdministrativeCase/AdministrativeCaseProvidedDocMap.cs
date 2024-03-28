/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Предоставляемые документы Административного дела ГЖИ"
///     /// </summary>
///     public class AdministrativeCaseProvidedDocMap : BaseEntityMap<AdministrativeCaseProvidedDoc>
///     {
///         public AdministrativeCaseProvidedDocMap()
///             : base("GJI_ADMINCASE_PROVDOC")
///         {
///             References(x => x.AdministrativeCase, "ADMINCASE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ProvidedDoc, "PROVIDED_DOC_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseProvidedDoc"</summary>
    public class AdministrativeCaseProvidedDocMap : BaseEntityMap<AdministrativeCaseProvidedDoc>
    {
        
        public AdministrativeCaseProvidedDocMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseProvidedDoc", "GJI_ADMINCASE_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AdministrativeCase, "AdministrativeCase").Column("ADMINCASE_ID").NotNull().Fetch();
            Reference(x => x.ProvidedDoc, "ProvidedDoc").Column("PROVIDED_DOC_ID").NotNull().Fetch();
        }
    }
}
