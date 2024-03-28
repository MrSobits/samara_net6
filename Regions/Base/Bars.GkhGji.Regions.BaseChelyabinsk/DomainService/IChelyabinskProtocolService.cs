namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс предоставляет возможность работать с требованиями и направлениями деятельности по протоколу
    /// </summary>
    public interface IChelyabinskProtocolService
    {
        /// <summary>
        /// Добавить требования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult AddRequirements(BaseParams baseParams);

        /// <summary>
        /// Добавить направления деятельности
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult AddDirections(BaseParams baseParams);

        /// <summary>
        /// Вернуть список текущих направлений деятельности по протоколу
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список направлений</returns>
        IDataResult ListDirections(BaseParams baseParams);
    }
}