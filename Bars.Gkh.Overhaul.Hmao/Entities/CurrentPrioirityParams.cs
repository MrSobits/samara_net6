namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Текущие параметры оценки
    /// </summary>
    public class CurrentPrioirityParams : BaseImportableEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public virtual int Order { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}