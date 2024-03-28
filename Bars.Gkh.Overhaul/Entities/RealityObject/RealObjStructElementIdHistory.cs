namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Идентификаторы конструктивных элементов дома, в том числ и удаленных (нужен для подтягивания истории изменений)
    /// </summary>
    public class RealObjStructElementIdHistory : BaseImportableEntity
    {
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Идентификатор конструктивного элемента
        /// </summary>
        public virtual long RealObjStructElId { get; set; }
    }
}