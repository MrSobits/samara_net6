namespace Bars.Gkh.Entities.CommonEstateObject
{
    using Bars.Gkh.Entities;

    using Enums;

    /// <summary>
    /// Атрибут группы конструктивных элементов
    /// </summary>
    public class StructuralElementGroupAttribute : BaseImportableEntity
    {
        /// <summary>
        /// Группа конструктивных элементов
        /// </summary>
        public virtual StructuralElementGroup Group { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Флаг: обязательность
        /// </summary>
        public virtual bool IsNeeded { get; set; }

        /// <summary>
        /// Тип атрибута
        /// </summary>
        public virtual AttributeType AttributeType { get; set; }

        /// <summary>
        /// Подсказка
        /// </summary>
        public virtual string Hint { get; set; }
    }
}