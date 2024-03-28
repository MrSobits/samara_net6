namespace Bars.Gkh.Entities.RealEstateType
{
    using Bars.Gkh.Entities;

    using CommonEstateObject;
    using Dicts;

    /// <summary>
    /// Конструктивные элемент типа домов
    /// </summary>
    public class RealEstateTypeStructElement: BaseImportableEntity
    {
        /// <summary>
        /// Тип домов
        /// </summary>
        public virtual RealEstateType RealEstateType { get; set; }

        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public virtual StructuralElement StructuralElement { get; set; }

        /// <summary>
        /// Флаг наличие/отсутствие элемента
        /// </summary>
        public virtual bool Exists { get; set; }
    }
}