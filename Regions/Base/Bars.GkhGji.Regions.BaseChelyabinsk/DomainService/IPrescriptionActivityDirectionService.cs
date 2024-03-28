namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс для работы с направлениями деятельности по предписаниям
    /// </summary>
    public interface IPrescriptionActivityDirectionService
    {
        /// <summary>
        /// Добавить направления деятельности
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult AddDirections(BaseParams baseParams);

        /// <summary>
        /// Вернуть список текущих направлений деятельности по предписанию
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список направлений</returns>
        IDataResult ListDirections(BaseParams baseParams);

    }
}