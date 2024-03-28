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
    using Entities;
    using DomainModelServices;

    /// <summary>
    /// Параметры распределения на дом
    /// </summary>
    public class DistributionByRealtyAccountArgs : AbstractDistributionArgs<DistributionByRealtyAccountArgs.ByRealtyRecord>
    {
        /// <summary>
        /// Распределяемый объект
        /// </summary>
        public IDistributable Distributable { get; private set; }

        /// <summary>
        /// Плательщик
        /// </summary>
        public string OriginatorName { get; set; }

        /// <summary>
        /// Цели распределения
        /// </summary>
        public override IEnumerable<ByRealtyRecord> DistributionRecords { get; protected set; }

        /// <summary>
        /// Сумма распределения (только для множественного распределения для подсчета финальной суммы)
        /// </summary>
        public decimal SumDistribution { get; protected set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <param name="distrRecords">Цели распределения</param>
        public DistributionByRealtyAccountArgs(IDistributable distributable, IEnumerable<ByRealtyRecord> distrRecords)
        {
            ArgumentChecker.NotNull(distributable, nameof(distributable));
            ArgumentChecker.NotNull(distrRecords, nameof(distrRecords));

            this.DistributionRecords = distrRecords;

            this.Distributable = distributable;
        }

        public DistributionByRealtyAccountArgs(IDistributable distributable, IEnumerable<ByRealtyRecord> distrRecords, decimal distrSum)
        {
            ArgumentChecker.NotNull(distributable, nameof(distributable));
            ArgumentChecker.NotNull(distrRecords, nameof(distrRecords));

            this.DistributionRecords = distrRecords;

            this.Distributable = distributable;

            this.SumDistribution = distrSum;
        }

        /// <summary>
        /// Получить параметры из параметров запроса
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Параметры распределения на дом</returns>
        public static DistributionByRealtyAccountArgs FromParams(BaseParams baseParams)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionId", "distributionId");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");

            var container = ApplicationContext.Current.Container;
            var realtyRepo = container.ResolveDomain<RealityObjectPaymentAccount>();

            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            using (container.Using(realtyRepo))
            {
                var records = baseParams.Params.GetAs<Proxy[]>("records");
                var realtiesIds = records.Select(x => x.RealtyAccountId).ToArray();

                var realties = realtyRepo.GetAll().Where(x => realtiesIds.Contains(x.Id)).ToArray();
                if (realties.Length != records.Length)
                {
                    throw new ArgumentException("records contains item with invalid realtyAccountId");
                }

                return new DistributionByRealtyAccountArgs(
                    distributable,
                    realties.Join(records,
                        realty => realty.Id,
                        proxy => proxy.RealtyAccountId,
                        (realty, proxy) => new ByRealtyRecord(realty, proxy.Sum)))
                {
                    OriginatorName = baseParams.Params.GetAs<string>("originatorName")
                };
            }
        }

        /// <summary>
        /// Получить параметры из параметров запроса
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Параметры распределения на дом</returns>
        public static DistributionByRealtyAccountArgs FromManyParams(BaseParams baseParams, int counter, decimal distribSum)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");

            var container = ApplicationContext.Current.Container;
            var realtyRepo = container.ResolveDomain<RealityObjectPaymentAccount>();

            var distributable = DistributionHelper.ExtractDistributable(baseParams, counter);

            using (container.Using(realtyRepo))
            {
                var records = baseParams.Params.GetAs<Proxy[]>("records");
                var realtiesIds = records.Select(x => x.RealtyAccountId).ToArray();

                var realties = realtyRepo.GetAll().Where(x => realtiesIds.Contains(x.Id)).ToArray();
                if (realties.Length != records.Length)
                {
                    throw new ArgumentException("records contains item with invalid realtyAccountId");
                }

                return new DistributionByRealtyAccountArgs(
                    distributable,
                    realties.Join(records,
                        realty => realty.Id,
                        proxy => proxy.RealtyAccountId,
                        (realty, proxy) => new ByRealtyRecord(realty, proxy.Sum)),
                    distribSum)
                {
                    OriginatorName = baseParams.Params.GetAs<string>("originatorName")
                };
            }
        }

        /// <summary>
        /// Распределение по дом
        /// </summary>
        public class ByRealtyRecord
        {
            /// <summary>
            /// Сумма
            /// </summary>
            public decimal Sum { get; private set; }

            /// <summary>
            /// Счет оплат дома
            /// </summary>
            public RealityObjectPaymentAccount RealtyAccount { get; private set; }

            /// <summary>
            /// Дата поступления
            /// </summary>
            public DateTime? DateReceipt { get; private set; }

            /// <summary>
            /// .ctor
            /// </summary>
            /// <param name="account">Счет оплат дома</param>
            /// <param name="sum">Сумма</param>
            /// <param name="dateReceipt">Дата поступления</param>
            public ByRealtyRecord(RealityObjectPaymentAccount account, decimal sum, DateTime? dateReceipt = null)
            {
                this.RealtyAccount = account;
                this.Sum = sum;
                this.DateReceipt = dateReceipt;
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Proxy
        {
            public long RealtyAccountId { get; set; }

            public decimal Sum { get; set; }
        }
    }
}