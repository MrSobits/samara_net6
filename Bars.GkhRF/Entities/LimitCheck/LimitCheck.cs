namespace Bars.GkhRf.Entities
{
    using Bars.Gkh.Entities;

    using Enums;

    /// <summary>
    /// Проверка лимитов по заявке
    /// </summary>
    public class LimitCheck : BaseImportableEntity
    {
        /// <summary>
        /// Тип программы заявки перечисления средств
        /// </summary>
        public virtual TypeProgramRequest TypeProgram { get; set; }
    }
}