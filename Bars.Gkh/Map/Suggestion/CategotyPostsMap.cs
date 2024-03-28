namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;


    /// <summary>
    /// Маппинг для "Категория сообщения"
    /// </summary>
    public class CategoryPostsMap : BaseImportableEntityMap<CategoryPosts>
    {

        public CategoryPostsMap() :
            base("Категория сообщения", "GKH_DICT_CATEGORY_POSTS")
        {
        }

        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE");
            Property(x => x.Name, "Name").Column("NAME");
        }
    }
}