namespace Bars.Gkh.RegOperator.Extenstions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Расширения для ЛС
    /// </summary>
    public static class BasePersonalAccountExtensions
    {
        /// <summary>
        /// Метод подгружает входящие трансферы для основных кошельков ЛС
        /// </summary>
        /// <typeparam name="TCollection">
        /// Тип коллекции с лицевыми счетами
        /// </typeparam>
        /// <param name="collection">
        /// Коллекция лицевых счетов
        /// </param>
        /// <param name="period">
        /// Период, за который необходимы трансферы
        /// </param>
        /// <returns>
        /// Коллекция лицевых счетов
        /// </returns>
        public static TCollection FetchMainWalletInTransfers<TCollection>(this TCollection collection, ChargePeriod period)
            where TCollection : IEnumerable<BasePersonalAccount>
        {
            collection.SelectMany(x => x.GetMainWallets()).FetchInTransfers(period);
            return collection;
        }

        /// <summary>
        /// Метод подгружает исходящие трансферы для основных кошельков ЛС
        /// </summary>
        /// <typeparam name="TCollection">
        /// Тип коллекции с лицевыми счетами
        /// </typeparam>
        /// <param name="collection">
        /// Коллекция лицевых счетов
        /// </param>
        /// <param name="period">
        /// Период, за который необходимы трансферы
        /// </param>
        /// <returns>
        /// Коллекция лицевых счетов
        /// </returns>
        public static TCollection FetchMainWalletOutTransfers<TCollection>(this TCollection collection, ChargePeriod period)
            where TCollection : IEnumerable<BasePersonalAccount>
        {
            collection.SelectMany(x => x.GetMainWallets()).FetchOutTransfers(period);
            return collection;
        }

        /// <summary>
        /// Метод подгружает исходящие трансферы для основных кошельков ЛС
        /// </summary>
        /// <typeparam name="TCollection">
        /// Тип коллекции с лицевыми счетами
        /// </typeparam>
        /// <param name="collection">
        /// Коллекция лицевых счетов
        /// </param>
        /// <param name="period">
        /// Период, за который необходимы трансферы
        /// </param>
        /// <param name="walletSelectors">
        /// Селекторы кошельков
        /// </param>
        /// <returns>
        /// Коллекция лицевых счетов
        /// </returns>
        public static TCollection FetchWalletOutTransfers<TCollection>(this TCollection collection, ChargePeriod period, params Func<BasePersonalAccount, Wallet>[] walletSelectors)
            where TCollection : IEnumerable<BasePersonalAccount>
        {
            ArgumentChecker.NotNullOrEmpty(walletSelectors, nameof(collection));

            Func<BasePersonalAccount, Wallet[]> walletSelector = x => walletSelectors.Select(y => y(x)).ToArray();
            collection.SelectMany(walletSelector).FetchOutTransfers(period);
            return collection;
        }

        /// <summary>
        /// Метод подгружает входящие трансферы для основных кошельков ЛС
        /// </summary>
        /// <typeparam name="TCollection">
        /// Тип коллекции с лицевыми счетами
        /// </typeparam>
        /// <param name="collection">
        /// Коллекция лицевых счетов
        /// </param>
        /// <param name="period">
        /// Период, за который необходимы трансферы
        /// </param>
        /// <param name="walletSelectors">
        /// Селекторы кошельков
        /// </param>
        /// <returns>
        /// Коллекция лицевых счетов
        /// </returns>
        public static TCollection FetchWalletInTransfers<TCollection>(this TCollection collection, ChargePeriod period, params Func<BasePersonalAccount, Wallet>[] walletSelectors)
            where TCollection : IEnumerable<BasePersonalAccount>
        {
            ArgumentChecker.NotNullOrEmpty(walletSelectors, nameof(collection));

            Func<BasePersonalAccount, Wallet[]> walletSelector = x => walletSelectors.Select(y => y(x)).ToArray();
            collection.SelectMany(walletSelector).FetchInTransfers(period);
            return collection;
        }

        /// <summary>
        /// Подгружает детализацию по текущему открытому периоду
        /// <para>Использовать вне <see cref="IQueryable{T}"/></para>
        /// </summary>
        /// <param name="collection">Коллекция</param>
        /// <param name="externalQuery">Внешний подзапрос фильтрации</param>
        /// <typeparam name="TCollection">Коллекция ЛС</typeparam>
        /// <returns>Исходная коллекция</returns>
        public static TCollection FetchCurrentOpenedPeriodSummary<TCollection>(
            this TCollection collection, 
            IQueryable<BasePersonalAccount> externalQuery = null)
            where TCollection : IEnumerable<BasePersonalAccount>
        {
            var container = ApplicationContext.Current.Container;
            var period = container.Resolve<IChargePeriodRepository>().GetCurrentPeriod();
            var accountIds = collection.Select(x => x.Id).ToArray();

            container.UsingForResolved<IDomainService<PersonalAccountPeriodSummary>>((cnt, service) =>
            {
                var periodSummDict = service.GetAll()
                    .Where(x => x.Period == period)
                    .WhereIf(externalQuery != null, x => externalQuery.Any(y => y == x.PersonalAccount))
                    .WhereIfContains(externalQuery == null, x => x.PersonalAccount.Id, accountIds)
                    .ToDictionary(x => x.PersonalAccount);

                foreach (var account in collection)
                {
                    account.SetOpenedPeriodSummary(periodSummDict.Get(account));
                }
            });

            return collection;
        }
    }
}