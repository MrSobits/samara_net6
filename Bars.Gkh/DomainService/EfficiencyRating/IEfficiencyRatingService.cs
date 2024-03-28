namespace Bars.Gkh.DomainService.EfficiencyRating
{
    using Bars.B4;
    using Bars.Gkh.DomainService.MetaValueConstructor;

    /// <summary>
    /// Сервис по работе с Рейтингом эффективности
    /// </summary>
    public interface IEfficiencyRatingService : IDataValueService
    {
        /// <summary>
        /// Метод возвращает все факторы, которые имеются в системе
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult GetFactors(BaseParams baseParams);
    }
}