namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Лог рассылки
    /// </summary>
    public class EmailNewsletterLog : BaseEntity
    {
        /// <summary>
        /// Рассылка
        /// </summary>
        public virtual EmailNewsletter EmailNewsletter { get; set; }

        /// <summary>
        /// Адресат
        /// </summary>
        public virtual string Destination { get; set; }

        /// <summary>
        /// Лог
        /// </summary>
        public virtual string Log { get; set; }

        /// <summary>
        /// Успешно?
        /// </summary>
        public virtual bool Success { get; set; }
    }
}