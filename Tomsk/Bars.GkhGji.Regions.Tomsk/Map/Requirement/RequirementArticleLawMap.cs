/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     public class RequirementArticleLawMap : BaseEntityMap<RequirementArticleLaw>
///     {
///         public RequirementArticleLawMap() : base("GJI_REQUIREMENT_ARTICLE")
///         {
///             References(x => x.ArticleLaw, "ARTICLE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Requirement, "REQ_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.RequirementArticleLaw"</summary>
    public class RequirementArticleLawMap : BaseEntityMap<RequirementArticleLaw>
    {
        
        public RequirementArticleLawMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.RequirementArticleLaw", "GJI_REQUIREMENT_ARTICLE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ArticleLaw, "ArticleLaw").Column("ARTICLE_ID").NotNull().Fetch();
            Reference(x => x.Requirement, "Requirement").Column("REQ_ID").NotNull().Fetch();
        }
    }
}
