namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;


    /// <summary>
    /// Маппинг для "Тема сообщения"
    /// </summary>
    public class MessageSubjectMap : BaseImportableEntityMap<MessageSubject>
    {

        public MessageSubjectMap() :
            base("Тема сообщения", "GKH_DICT_MESSAGE_SUBJECT")
        {
        }

        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE");
            Property(x => x.Name, "Name").Column("NAME");
            Reference(x => x.CategoryPosts, "Категория сообщения").Column("CATEGORY_POSTS_ID").NotNull().Fetch();
        }
    }
}