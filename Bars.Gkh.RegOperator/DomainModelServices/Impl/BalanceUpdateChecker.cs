namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Entities.ValueObjects;
    using Entities.Wallet;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Microsoft.Extensions.Logging;

    using Bars.Gkh.Domain.DatabaseMutex;

    using Castle.MicroKernel.Lifestyle;

    public class BalanceUpdateChecker : IBalanceUpdateChecker
    {
        private IWindsorContainer _container;
        private IRepository<BasePersonalAccount> _accountRepo;
        private IRepository<RealityObjectPaymentAccount> _paymentRepo;
        private IRepository<RealityObject> _realityObjectRepo;
        private IRepository<Transfer> _transferRepo;
        private IRealityObjectAccountUpdater _updater;

        public BalanceUpdateChecker(IWindsorContainer container)
        {
            _container = container;
        }

        public void PerformUpdate()
        {
            _accountRepo = _container.Resolve<IRepository<BasePersonalAccount>>();
            _paymentRepo = _container.Resolve<IRepository<RealityObjectPaymentAccount>>();
            _realityObjectRepo = _container.Resolve<IRepository<RealityObject>>();
            _updater = _container.Resolve<IRealityObjectAccountUpdater>();
            var logger = _container.Resolve<ILogger>();
            var mutexManager = _container.Resolve<IDatabaseMutexManager>();
            var sessions = _container.Resolve<ISessionProvider>();

            IDatabaseLockedMutexHandle handle;

            logger.LogInformation("Старт расчета балансов");

            _container.InTransaction(
                () =>
                {
                    if (
                        !mutexManager.TryLock(
                            "BalanceUpdater",
                            "Обновление балансов домов в текущем месяце",
                            out handle))
                    {
                        logger.LogInformation("Уже идет другой расчет. Выход");
                        return;
                    }

                    using (handle)
                    using (_container.Using(_accountRepo, _paymentRepo, _realityObjectRepo, _transferRepo, _updater))
                    {
                        logger.LogInformation("Получение информации по изменениям для ЛС");

                        var rosFromAccs = sessions.GetCurrentSession().CreateSQLQuery(@"select distinct ro.id
from regop_pers_acc acc
left join regop_wallet w1 on w1.id = acc.bt_wallet_id and w1.has_new_ops = TRUE
left join regop_wallet w2 on w2.id = acc.dt_wallet_id and w2.has_new_ops = TRUE
left join regop_wallet w3 on w3.id = acc.af_wallet_id and w3.has_new_ops = TRUE
left join regop_wallet w4 on w4.id = acc.pwp_wallet_id and w4.has_new_ops = TRUE
left join regop_wallet w5 on w5.id = acc.p_wallet_id and w5.has_new_ops = TRUE
left join regop_wallet w6 on w6.id = acc.r_wallet_id and w6.has_new_ops = TRUE
left join regop_wallet w7 on w7.id = acc.ss_wallet_id and w7.has_new_ops = TRUE
join gkh_room room on acc.room_id = room.id
join gkh_reality_object ro on ro.id = room.ro_id").List<object>().Select(x => x.ToLong());

                        var rosFromPayments = sessions.GetCurrentSession().CreateSQLQuery(@"select distinct ro.id
from regop_ro_payment_account acc
left join regop_wallet w1 on 	w1 .id = acc.bt_wallet_id 		and w1 .has_new_ops = TRUE
left join regop_wallet w2 on 	w2 .id = acc.dt_wallet_id 		and w2 .has_new_ops = TRUE
left join regop_wallet w3 on 	w3 .id = acc.p_wallet_id			and w3 .has_new_ops = TRUE
left join regop_wallet w4 on 	w4 .id = acc.fsu_wallet_id 		and w4 .has_new_ops = TRUE
left join regop_wallet w5 on 	w5 .id = acc.rsu_wallet_id		and w5 .has_new_ops = TRUE
left join regop_wallet w6 on 	w6 .id = acc.ssu_wallet_id		and w6 .has_new_ops = TRUE
left join regop_wallet w7 on 	w7 .id = acc.tsu_wallet_id		and w7 .has_new_ops = TRUE
left join regop_wallet w8 on 	w8 .id = acc.os_wallet_id 		and w8 .has_new_ops = TRUE
left join regop_wallet w9 on 	w9 .id = acc.bp_wallet_id 		and w9 .has_new_ops = TRUE
left join regop_wallet w10 on w10.id = acc.af_wallet_id 		and w10.has_new_ops = TRUE
left join regop_wallet w11 on w11.id = acc.pwp_wallet_id		and w11.has_new_ops = TRUE
left join regop_wallet w12 on w12.id = acc.r_wallet_id			and w12.has_new_ops = TRUE
left join regop_wallet w13 on w13.id = acc.ss_wallet_id 		and w13.has_new_ops = TRUE
join gkh_reality_object ro on ro.id = acc.ro_id").List<object>().Select(x => x.ToLong());

                        logger.LogInformation("Получение информации по изменениям для счетов оплат дома");

                        var allRoIds = rosFromAccs.Union(rosFromPayments).Distinct().ToList();

                        foreach (var roIds in allRoIds.Section(500))
                        {
                            logger.LogInformation("Расчет балансов для {0} домов".FormatUsing(roIds.Count()));

                            var ros = _realityObjectRepo.GetAll().Where(x => roIds.Contains(x.Id));

                            _updater.UpdateBalance(ros);
                        }
                    }
                });
        }

        private Func<T, bool> CreateWhereExpression<T>()
        {
            var props = typeof (T).GetProperties().Where(x => x.PropertyType == typeof (Wallet)).ToArray();

            var holderParam = Expression.Parameter(typeof (T));
            
            Expression whereExpr = null;

            foreach (var wallet in props)
            {
                var walletProp = Expression.Property(holderParam, wallet);
                var walletNewProp = Expression.Property(walletProp, "HasNewOperations");

                var equalsExpr = Expression.Equal(walletNewProp, Expression.Constant(true));
                
                if (whereExpr == null)
                {
                    whereExpr = equalsExpr;
                }
                else
                {
                    whereExpr = Expression.OrElse(whereExpr, equalsExpr);
                }
            }

            return Expression.Lambda<Func<T, bool>>(whereExpr, holderParam).Compile();
        }

        /// <summary>
        /// Выполнение задачи
        /// </summary>
        /// <param name="params">Параметры исполнения задачи.
        ///             При вызове из планировщика передаются параметры из JobDataMap 
        ///             и контекст исполнения в параметре JobContext        
        ///             </param>
        public void Execute(DynamicDictionary @params)
        {
            using (this._container.BeginScope())
            {
                PerformUpdate();
            }
        }
    }
}