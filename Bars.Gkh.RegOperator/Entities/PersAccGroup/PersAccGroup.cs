namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Группа лицевых счетов
    /// </summary>
    public class PersAccGroup : BaseEntity
    {
        /// <summary>
        /// Наименование группы
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Признак системности группы
        /// </summary>
        public virtual YesNo IsSystem { get; set; }
    }
}
