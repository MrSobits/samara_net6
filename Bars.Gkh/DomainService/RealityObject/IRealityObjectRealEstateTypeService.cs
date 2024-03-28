namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    public interface IRealityObjectRealEstateTypeService
    {
        /// <summary>
        /// Возвращает типы домов, если параметр стоит "Автоматический", иначе Success = false
        /// </summary>
        IDataResult GetAutoRealEstateType(RealityObject realityObject);
    }
}