namespace Bars.Gkh.DomainService.EfficiencyRating
{
    using Bars.B4;

    /// <summary>
    /// Сервис для работы с аналитикой Рейтинга эффективности
    /// </summary>
    public interface IEfficiencyRatingAnaliticsGraphService
    {
        /// <summary>
        /// Метод возвращает график по параметрам
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>результат запроса</returns>
        IDataResult GetGraph(BaseParams baseParams);
        
        /// <summary>
        /// Метод сохраняет или создает график
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>результат запроса</returns>
        IDataResult SaveOrUpdateGraph(BaseParams baseParams);
    }
}