namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    /// Работа по конструктивному элементу
    /// </summary>
    public class StructuralElementWork : BaseImportableEntity
    {
        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public virtual StructuralElement StructuralElement { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public virtual Job Job { get; set; }
    }
}