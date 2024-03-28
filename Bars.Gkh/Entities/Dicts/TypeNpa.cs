namespace Bars.Gkh.Entities.Dicts
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Типы НПА
    /// </summary>
    public class TypeNpa : BaseEntity
    {
        /// <summary>
        /// Тип НПА
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}