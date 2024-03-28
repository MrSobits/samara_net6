namespace Bars.Gkh.Entities
{
    using Bars.Gkh.ImportExport;

    using Dicts;

    /// <summary>
    /// Конструктивный элемент
    /// </summary>
    public class ConstructiveElement : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Группа старая (удалить)
        /// </summary>
        public virtual string Grp { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public virtual ConstructiveElementGroup Group { get; set; }

        /// <summary>
        /// Срок эксплуатации
        /// </summary>
        public virtual decimal Lifetime { get; set; }

        /// <summary>
        /// Нормативный документ
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Стоимость ремонта
        /// </summary>
        public virtual decimal? RepairCost { get; set; }
    }
}