namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Категория документации
    /// </summary>
    public class InstructionGroup : BaseEntity
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public virtual string DisplayName { get; set; }
    }
}