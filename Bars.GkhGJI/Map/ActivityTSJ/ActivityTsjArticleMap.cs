/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Статья устава деятельности ТСЖ"
///     /// </summary>
///     public class ActivityTsjArticleMap : BaseGkhEntityMap<ActivityTsjArticle>
///     {
///         public ActivityTsjArticleMap() : base("GJI_ACT_TSJ_ARTICLE")
///         {
///             Map(x => x.IsNone, "IS_NONE");
///             Map(x => x.Paragraph, "PARAGRAPH").Length(1000);
///             Map(x => x.TypeState, "TYPE_STATE").Not.Nullable().CustomType<TypeState>();
/// 
///             References(x => x.ActivityTsjStatute, "ACT_TSJ_STATUATE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ArticleTsj, "ARTICLE_TSJ_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Статья устава деятельности ТСЖ"</summary>
    public class ActivityTsjArticleMap : BaseEntityMap<ActivityTsjArticle>
    {
        
        public ActivityTsjArticleMap() : 
                base("Статья устава деятельности ТСЖ", "GJI_ACT_TSJ_ARTICLE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.IsNone, "Отсутствует").Column("IS_NONE");
            Property(x => x.Paragraph, "Пункт устава").Column("PARAGRAPH").Length(1000);
            Property(x => x.TypeState, "Пункт устава").Column("TYPE_STATE").NotNull();
            Reference(x => x.ActivityTsjStatute, "Устав деятельности ТСЖ").Column("ACT_TSJ_STATUATE_ID").NotNull().Fetch();
            Reference(x => x.ArticleTsj, "Статья ТСЖ").Column("ARTICLE_TSJ_ID").NotNull().Fetch();
        }
    }
}
