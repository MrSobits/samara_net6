namespace Bars.Gkh.Entities
{
    using Bars.Gkh.ImportExport;

    using Dicts;

    /// <summary>
    /// Группа конструктивного элемента
    /// </summary>
    public class ConstructiveElementGroup : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Обязательность
        /// </summary>
        public virtual bool Necessarily { get; set; }
    }
}
