namespace Bars.Gkh.Entities.Dicts
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Типы информации в НПА
    /// </summary>
    public class TypeInformationNpa : BaseEntity
    {
        /// <summary>
        /// Тип информации
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public virtual CategoryInformationNpa Category { get; set; }
    }
}