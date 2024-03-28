namespace Bars.Gkh.Entities.Suggestion
{
    /// <summary>
    /// Категория сообщения
    /// </summary>
    public class CategoryPosts : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
