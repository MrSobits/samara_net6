namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System.Linq;
    using Gkh.Entities;

    public interface IRealityObjectAccountUpdater
    {
        /// <summary>
        /// Обновить баланс домов 
        /// </summary>
        /// <param name="realityObjects">Запрос-фильтр домов</param>
        /// <param name="accountIds">Список лс, которые нужно пересчитать</param>
        void UpdateBalance(IQueryable<RealityObject> realityObjects, long[] accountIds = null);

        /// <summary>
        /// Обновить кредит и дебет 
        /// </summary>
        /// <param name="realityObjects">Запрос-фильтр домов</param>
        void UpdateCreditsAndDebts(IQueryable<RealityObject> realityObjects);
    }
}