namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Роль для категории
    /// </summary>
    public class InstructionGroupRole : BaseEntity
    {
        /// <summary>
        /// Категория документации
        /// </summary>
        public virtual InstructionGroup InstructionGroup { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }
    }
}