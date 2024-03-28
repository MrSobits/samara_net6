/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Статья ТСЖ ГЖИ"
///     /// </summary>
///     public class ArticleTsjMap : BaseGkhEntityMap<ArticleTsj>
///     {
///         public ArticleTsjMap() : base("GJI_DICT_ARTICLE_TSJ")
///         {
///             Map(x => x.Name, "NAME").Length(1000).Not.Nullable();
///             Map(x => x.Group, "GROUP_NAME").Length(300);
///             Map(x => x.Code, "CODE").Length(50);
///             Map(x => x.Article, "ARTICLE").Length(250);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Статья ТСЖ"</summary>
    public class ArticleTsjMap : BaseEntityMap<ArticleTsj>
    {
        
        public ArticleTsjMap() : 
                base("Статья ТСЖ", "GJI_DICT_ARTICLE_TSJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(1000).NotNull();
            Property(x => x.Group, "Группа").Column("GROUP_NAME").Length(300);
            Property(x => x.Code, "Код").Column("CODE").Length(50);
            Property(x => x.Article, "Статья ЖК").Column("ARTICLE").Length(250);
        }
    }
}
