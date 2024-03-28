namespace Bars.Gkh.RegOperator.Distribution.Args
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils.Annotations;

    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Extenstions;

    using DomainModelServices;
    using Entities;
    using Enums;

    using NHibernate.Linq;

    /// <summary>
    /// Параметры распределения на ЛС
    /// </summary>
    public class DistributionByAccountsArgs : AbstractDistributionArgs<DistributionByAccountsArgs.ByPersAccountRecord>
    {
        /// <summary>
        /// Распределяемый объект
        /// </summary>
        public IDistributable Distributable { get; private set; }
       
        /// <summary>
        /// Цели распределения
        /// </summary>
        public override IEnumerable<ByPersAccountRecord> DistributionRecords { get; protected set; }

        /// <summary>
        /// Вид распределения
        /// </summary>
        public SuspenseAccountDistributionParametersView TypeDistribution { get; protected set; }

        /// <summary>
        /// Сумма распределения (только для множественного распределения для подсчета финальной суммы)
        /// </summary>
        public decimal SumDistribution { get; protected set; }

        /// <summary>
        /// Дополнительные параметры
        /// </summary>
        public Dictionary<string, object> Params { get; protected set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <param name="extraParams">Дополнительные параметры</param>
        /// <param name="typeDistribution">Вид распределения</param>
        /// <param name="distrRecords">Цели распределения</param>
        /// <param name="distrSum">Сумма распределения</param>
        public DistributionByAccountsArgs(IDistributable distributable, Dictionary<string, object> extraParams, SuspenseAccountDistributionParametersView typeDistribution, IEnumerable<ByPersAccountRecord> distrRecords, decimal distrSum)
        {
            ArgumentChecker.NotNull(distributable, nameof(distributable));
            ArgumentChecker.NotNull(distrRecords, nameof(distrRecords));

            this.Params = extraParams;

            this.Distributable = distributable;
            this.DistributionRecords = distrRecords;
            this.TypeDistribution = typeDistribution;
            this.SumDistribution = distrSum;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <param name="extraParams">Дополнительные параметры</param>
        /// <param name="typeDistribution">Вид распределения</param>
        /// <param name="distrRecords">Цели распределения</param>
        public DistributionByAccountsArgs(IDistributable distributable, Dictionary<string, object> extraParams, SuspenseAccountDistributionParametersView typeDistribution, IEnumerable<ByPersAccountRecord> distrRecords)
        {
            ArgumentChecker.NotNull(distributable, nameof(distributable));
            ArgumentChecker.NotNull(distrRecords, nameof(distrRecords));

            this.Params = extraParams;

            this.Distributable = distributable;
            this.DistributionRecords = distrRecords;
            this.TypeDistribution = typeDistribution;
        }


        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <param name="distributeOn">Распределить по</param>
        /// <param name="distrRecords">Цели распределения</param>
        public DistributionByAccountsArgs(IDistributable distributable, DistributeOn distributeOn, IEnumerable<ByPersAccountRecord> distrRecords)
        {
            ArgumentChecker.NotNull(distributable, nameof(distributable));
            ArgumentChecker.NotNull(distributeOn, nameof(distributeOn));
            ArgumentChecker.NotNull(distrRecords, nameof(distrRecords));

            this.Params = new Dictionary<string, object>();
            this.Params.Add("DistributeOn", distributeOn);

            this.Distributable = distributable;
            this.DistributionRecords = distrRecords;
        }

        /// <summary>
        /// Получить параметры из параметров запроса
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Параметры распределения на ЛС</returns>
        public static DistributionByAccountsArgs FromParams(BaseParams baseParams)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionId", "distributionId");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");
            
            var distributionView = baseParams.Params.GetAs<SuspenseAccountDistributionParametersView>("distributionView");

            var parametrs = new [] {"periodId", "snapshotId", "distributeOn" };

            var extraParams = baseParams.Params.Where(x => parametrs.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            var container = ApplicationContext.Current.Container;
            var persAccDomain = container.ResolveDomain<BasePersonalAccount>();

            using (container.Using(persAccDomain))
            {
                var records = baseParams.Params.GetAs<List<Proxy>>("records");
                var accIds = records.Select(x => x.AccountId).ToArray();

                var accs = persAccDomain.GetAll()
                    .FetchAllWallets()
                    .Fetch(x => x.Room)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => accIds.Contains(x.Id))
                    .ToList()
                    .FetchCurrentOpenedPeriodSummary();

                if (accs.Count != records.Count)
                {
                    throw new ArgumentException("records contains item with invalid AccId");
                }

                return new DistributionByAccountsArgs(
                    distributable,
                    extraParams,
                    distributionView,
                    accs.Join(records,
                        account => account.Id,
                        proxy => proxy.AccountId,
                        (act, proxy) => new ByPersAccountRecord(act, proxy.Sum, proxy.SumPenalty)));
            }
        }

        /// <summary>
        /// Получить параметры из параметров множества запросов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Параметры распределения на ЛС</returns>
        public static DistributionByAccountsArgs FromManyParams(BaseParams baseParams, int counter, decimal distribSum)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");

            var distributionView = baseParams.Params.GetAs<SuspenseAccountDistributionParametersView>("distributionView");

            var parametrs = new[] { "periodId", "snapshotId", "distributeOn" };

            var extraParams = baseParams.Params.Where(x => parametrs.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

            var distributable = DistributionHelper.ExtractDistributable(baseParams, counter);

            var container = ApplicationContext.Current.Container;
            var persAccDomain = container.ResolveDomain<BasePersonalAccount>();

            using (container.Using(persAccDomain))
            {
                var records = baseParams.Params.GetAs<List<Proxy>>("records");
                var accIds = records.Select(x => x.AccountId).ToArray();

                var accs = persAccDomain.GetAll()
                    .FetchAllWallets()
                    .Fetch(x => x.Room)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => accIds.Contains(x.Id))
                    .ToList()
                    .FetchCurrentOpenedPeriodSummary();

                if (accs.Count != records.Count)
                {
                    throw new ArgumentException("records contains item with invalid AccId");
                }

                return new DistributionByAccountsArgs(
                    distributable,
                    extraParams,
                    distributionView,
                    accs.Join(records,
                        account => account.Id,
                        proxy => proxy.AccountId,
                        (act, proxy) => new ByPersAccountRecord(act, proxy.Sum, proxy.SumPenalty)),
                    distribSum);
            }
        }

        /// <summary>
        /// Распределение по ЛС
        /// </summary>
        public sealed class ByPersAccountRecord
        {
            /// <summary>
            /// .ctor
            /// </summary>
            /// <param name="personalAccount">Лицевой счет</param>
            /// <param name="sum">Сумма</param>
            /// <param name="sumPenalty">Сумма пени</param>
            public ByPersAccountRecord(BasePersonalAccount personalAccount, decimal sum, decimal sumPenalty)
            {
                this.PersonalAccount = personalAccount;
                this.Sum = sum;
                this.SumPenalty = sumPenalty;
            }

            /// <summary>
            /// Лицевой счет
            /// </summary>
            public BasePersonalAccount PersonalAccount { get; private set; }

            /// <summary>
            /// Сумма
            /// </summary>
            public decimal Sum { get; private set; }

            /// <summary>
            /// Сумма пени
            /// </summary>
            public decimal SumPenalty { get; private set; }
        }

        /// <summary>
        /// Класс, определяющий формат распределения который приходит с клиента.
        /// В соответствии с этим форматом будут извлекаться данные из BaseParams.
        /// </summary>
        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Proxy
        {
            public long AccountId { get; set; }

            public decimal Sum { get; set; }

            public decimal SumPenalty { get; set; }
        }
    }
}
