namespace Bars.Gkh.RegOperator.Distribution.Args
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using B4.Utils.Annotations;
    using DomainModelServices;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using NHibernate.Linq;

    public class DistributionByTransferCtrArgs : AbstractDistributionArgs<ByTransferCtrRecord>
    {
        public IDistributable Distributable { get; private set; }

        public override IEnumerable<ByTransferCtrRecord> DistributionRecords { get; protected set; }

        /// <summary>
        /// Сумма распределения (только для множественного распределения для подсчета финальной суммы)
        /// </summary>
        public decimal SumDistribution { get; protected set; }

        private DistributionByTransferCtrArgs(IDistributable distributable, IEnumerable<ByTransferCtrRecord> records)
        {
            this.Distributable = distributable;
            this.DistributionRecords = records;
        }

        private DistributionByTransferCtrArgs(IDistributable distributable, IEnumerable<ByTransferCtrRecord> records, decimal distrSum)
        {
            this.Distributable = distributable;
            this.DistributionRecords = records;
            this.SumDistribution = distrSum;
        }

        public static DistributionByTransferCtrArgs FromParams(BaseParams baseParams)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionId", "distributionId");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");

            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            var container = ApplicationContext.Current.Container;
            var transferCtrDomain = container.ResolveDomain<TransferCtr>();

            using (container.Using(transferCtrDomain))
            {
                var proxies = baseParams.Params.GetAs<Proxy[]>("records");

                var ids = proxies.Select(x => x.Id).ToArray();

                var transfers = transferCtrDomain.GetAll()
                    .Where(x => ids.Contains(x.Id))
                    .Fetch(x => x.ObjectCr)
                    .ThenFetch(x => x.RealityObject)
                    .ToDictionary(x => x.Id);

                if (transfers.Count != proxies.Length)
                {
                    throw new ArgumentException("records contains item with invalid TransferCtrId");
                }

                var records = proxies
                    .Select(x => new ByTransferCtrRecord
                    {
                        Sum = x.Sum,
                        TransferCtr = transfers.Get(x.Id)
                    })
                    .ToArray();

                return new DistributionByTransferCtrArgs(distributable, records);
            }
        }

        public static DistributionByTransferCtrArgs FromManyParams(BaseParams baseParams, int counter, decimal distribSum)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");

            var distributable = DistributionHelper.ExtractDistributable(baseParams, counter);

            var container = ApplicationContext.Current.Container;
            var transferCtrDomain = container.ResolveDomain<TransferCtr>();

            using (container.Using(transferCtrDomain))
            {
                var proxies = baseParams.Params.GetAs<Proxy[]>("records");

                var ids = proxies.Select(x => x.Id).ToArray();

                var transfers = transferCtrDomain.GetAll()
                    .Where(x => ids.Contains(x.Id))
                    .Fetch(x => x.ObjectCr)
                    .ThenFetch(x => x.RealityObject)
                    .ToDictionary(x => x.Id);

                if (transfers.Count != proxies.Length)
                {
                    throw new ArgumentException("records contains item with invalid TransferCtrId");
                }

                var records = proxies
                    .Select(x => new ByTransferCtrRecord
                    {
                        Sum = x.Sum,
                        TransferCtr = transfers.Get(x.Id)
                    })
                    .ToArray();

                return new DistributionByTransferCtrArgs(distributable, records, distribSum);
            }
        }

        private class Proxy
        {
            public long Id { get; set; }

            public decimal Sum { get; set; }
        }
    }

    public sealed class ByTransferCtrRecord
    {
        public TransferCtr TransferCtr { get; set; }

        public decimal Sum { get; set; }
    }
}