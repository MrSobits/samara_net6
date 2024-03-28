namespace Bars.Gkh.RegOperator.Domain
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.Gkh.RegOperator.Entities;

    using Entities.Wallet;
    using NHibernate.Linq;

    public static class WalletQueryExtensions
    {
        public static IQueryable<THolder> FetchWalletWithTransfers<THolder>(this IQueryable<THolder> query,
                Expression<Func<THolder, Wallet>> wallet)
        {
            return query.Fetch(wallet)
                .ThenFetchMany(x => x.InTransfers)
                .ThenFetch(x => x.Operation)
                .Fetch(wallet)
                .ThenFetchMany(x => x.OutTransfers)
                .ThenFetch(x => x.Operation)
                .Fetch(wallet)
                .ThenFetchMany(x => x.MoneyLocks);
        }

        /// <summary>
        /// Подгрузить все кошельки
        /// 
        /// <para>Внимание! не использовать при соединении с трансферами, Postgresql сходит с ума и строит ужасные планы запросов</para>
        /// </summary>
        public static IQueryable<THolder> FetchAllWallets<THolder>(this IQueryable<THolder> query)
        {
            var walletProps = typeof (THolder).GetProperties().Where(x => x.PropertyType == typeof (Wallet));
            var paramExpr = Expression.Parameter(typeof (THolder));

            foreach (var wallet in walletProps)
            {
                var propExpr = Expression.Property(paramExpr, wallet);
                var lambda = Expression.Lambda<Func<THolder, Wallet>>(propExpr, paramExpr);

                query = query.Fetch(lambda);
            }

            return query;
        }
    }
}