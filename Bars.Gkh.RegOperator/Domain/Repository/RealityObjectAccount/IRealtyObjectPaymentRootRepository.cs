namespace Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount
{
    using System.Collections.Generic;
    using AggregationRoots;
    using Gkh.Entities;

    /// <summary>
    /// Интерфейс репозитория для корней агрегации счетов дома
    /// </summary>
    public interface IRealtyObjectPaymentRootRepository
    {
        /// <summary>
        /// Получить корни агррегации по домам
        /// </summary>
        /// <param name="realityObjects">Дома</param>
        /// <returns></returns>
        IEnumerable<RealtyObjectPaymentRoot> GetByRealityObjects(IEnumerable<RealityObject> realityObjects);

        /// <summary>
        /// Обновить корень аггрегации
        /// </summary>
        /// <param name="root">Корень</param>
        void Update(RealtyObjectPaymentRoot root);
    }
}