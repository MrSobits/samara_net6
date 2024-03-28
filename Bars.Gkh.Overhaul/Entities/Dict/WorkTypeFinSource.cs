namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.Enum;

    /// <summary>
    /// Источник финансирования вида работ
    /// </summary>
    public class WorkTypeFinSource : BaseImportableEntity
    {
        /// <summary>
        /// Работа
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Тип источника финансирования
        /// </summary>
        public virtual TypeFinSource TypeFinSource { get; set; }
    }
}