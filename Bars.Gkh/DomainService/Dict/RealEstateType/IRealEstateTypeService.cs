namespace Bars.Gkh.DomainService.Dict.RealEstateType
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;

    using Entities;

    /// <summary>
    /// Сервис для работы с типами дома
    /// Реализуется в регионах(Nso, Hmao и т.д)
    /// </summary>
    public interface IRealEstateTypeService
    {
        /// <summary>
        /// Получить типы домов
        /// </summary>
        /// <param name="roQuery">Запрос жилых домов</param>
        /// <returns>Типы домов</returns>
        Dictionary<long, List<long>> GetRealEstateTypes(IQueryable<RealityObject> roQuery);

        /// <summary>
        /// Обновить типы домов
        /// </summary>
        /// <returns>Успешность</returns>
        IDataResult UpdateRobjectTypes();

        /// <summary>
        /// Предпросмотр обновления типов домов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Список обновляемых домов</returns>
        IDataResult UpdateRobjectTypesPreview(BaseParams baseParams);
    }
}