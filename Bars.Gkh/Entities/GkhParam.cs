namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;

    public class GkhParam : BaseEntity
    {
        /// <summary>
        /// Префикс
        /// </summary>
        public virtual string Prefix { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }
    }
}