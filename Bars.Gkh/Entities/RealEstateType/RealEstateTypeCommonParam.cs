namespace Bars.Gkh.Entities.RealEstateType
{
    using Bars.Gkh.Entities;

    using Dicts;

    /// <summary>
    /// Общий параметр типа жилых домов
    /// </summary>
    public class RealEstateTypeCommonParam: BaseImportableEntity
    {
        /// <summary>
        /// Тип домов
        /// </summary>
        public virtual RealEstateType RealEstateType { get; set; }

        /// <summary>
        /// Код общего параметра
        /// </summary>
        public virtual string CommonParamCode { get; set; }

        /// <summary>
        /// Минимальное значение
        /// </summary>
        public virtual string Min { get; set; }

        /// <summary>
        /// Максимальное значение
        /// </summary>
        public virtual string Max { get; set; }
    }
}