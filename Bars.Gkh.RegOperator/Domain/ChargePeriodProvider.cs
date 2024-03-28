namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Threading;

    using Bars.B4.Application;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Провайдер периодов
    /// </summary>
    public static class ChargePeriodProvider
    {
        private static readonly Lazy<IChargePeriodRepository> chargePeriodRepositoryLazy = new Lazy<IChargePeriodRepository>(ChargePeriodProvider.Init, LazyThreadSafetyMode.ExecutionAndPublication); 

        private static IChargePeriodRepository Init()
        {
            var container = ApplicationContext.Current.Container;
            return container.Resolve<IChargePeriodRepository>();
        }

        /// <summary>
        /// Получить экземпляр <see cref="IChargePeriodRepository"/>
        /// </summary>
        public static IChargePeriodRepository Repository => ChargePeriodProvider.chargePeriodRepositoryLazy.Value;

        /// <summary>
        /// Получить текущий открытый период
        /// </summary>
        /// <param name="useCache">Использовать кэш</param>
        /// <returns>Текущий открытый период</returns>
        public static ChargePeriod GetCurrentPeriod(bool useCache = true) => ChargePeriodProvider.Repository.GetCurrentPeriod(useCache);
    }
}