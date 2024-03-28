/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Статьи закона административного дела ГЖИ"
///     /// </summary>
///     public class AdministrativeCaseArticleLawMap : BaseEntityMap<AdministrativeCaseArticleLaw>
///     {
///         public AdministrativeCaseArticleLawMap()
///             : base("GJI_ADMINCASE_ARTLAW")
///         {
///             References(x => x.AdministrativeCase, "ADMINCASE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ArticleLaw, "ARTICLELAW_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseArticleLaw"</summary>
    public class AdministrativeCaseArticleLawMap : BaseEntityMap<AdministrativeCaseArticleLaw>
    {
        
        public AdministrativeCaseArticleLawMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.AdministrativeCaseArticleLaw", "GJI_ADMINCASE_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AdministrativeCase, "AdministrativeCase").Column("ADMINCASE_ID").NotNull().Fetch();
            Reference(x => x.ArticleLaw, "ArticleLaw").Column("ARTICLELAW_ID").NotNull().Fetch();
        }
    }
}
