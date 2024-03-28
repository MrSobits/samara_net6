namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Инструкция
    /// </summary>
    public class Instruction : BaseEntity
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Файл инструкции
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Категория документации
        /// </summary>
        public virtual InstructionGroup InstructionGroup { get; set; }
    }
}