namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Единица измерения
    /// </summary>
    public class UnitMeasure : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Короткое наименоваание
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Код ОКЕИ
        /// </summary>
        public virtual string OkeiCode { get; set; }
    }
}
