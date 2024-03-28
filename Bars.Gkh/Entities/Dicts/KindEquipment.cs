namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Вид оснащения
    /// </summary>
    public class KindEquipment : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Ед. измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }
    }
}
