namespace Bars.Gkh.Entities.Suggestion
{
    /// <summary>
    /// Тема сообщения
    /// </summary>
    public class MessageSubject : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Категория сообщения
        /// </summary>
        public virtual CategoryPosts CategoryPosts { get; set; }
    }
}

