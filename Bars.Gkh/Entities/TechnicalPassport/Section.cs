namespace Bars.Gkh.Entities.TechnicalPassport
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Секция
    /// </summary>
    public class Section : BaseEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual int Order { get; set; }

        /// <summary>
        /// Родительская секция
        /// </summary>
        public virtual Section Parent { get; set; }
    }
}