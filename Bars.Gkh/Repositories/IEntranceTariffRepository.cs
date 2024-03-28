namespace Bars.Gkh.Repositories
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Репозиторий тарифов по подъездам
    /// </summary>
    public interface IEntranceTariffRepository
    {
        /// <summary>
        /// Получить тарифы для всех подъездов дома
        /// </summary>
        Dictionary<long, decimal> GetRobjectTariff(long roId, DateTime date);

        /// <summary>
        /// Получить тариф для указанного дома и указанного типа дома
        /// </summary>
        decimal GetRetTariff(long roId, long retId, DateTime date);
    }
}