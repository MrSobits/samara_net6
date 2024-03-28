namespace Bars.Gkh.RegOperator.Distribution.Args
{
    using System;
    using System.Collections;
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

    public class DistributionByRefundTransferCtrArgs : AbstractDistributionArgs<ByRefundTransferCtrRecord>
    {
        public IDistributable Distributable { get; private set; }

        public override IEnumerable<ByRefundTransferCtrRecord> DistributionRecords { get; protected set; }

        /// <summary>
        /// Сумма распределения (только для множественного распределения для подсчета финальной суммы)
        /// </summary>
        public decimal SumDistribution { get; protected set; }

        private DistributionByRefundTransferCtrArgs(IDistributable distributable, IEnumerable<ByRefundTransferCtrRecord> records)
        {
            this.Distributable = distributable;
            this.DistributionRecords = records;
        }

        private DistributionByRefundTransferCtrArgs(IDistributable distributable, IEnumerable<ByRefundTransferCtrRecord> records, decimal distrSum)
        {
            this.Distributable = distributable;
            this.DistributionRecords = records;
            this.SumDistribution = distrSum;
        }

        public static DistributionByRefundTransferCtrArgs FromParams(BaseParams baseParams)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionId", "distributionId");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");

            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            var container = ApplicationContext.Current.Container;
            var transferCtrDetailDomain = container.ResolveDomain<TransferCtrPaymentDetail>();

            using (container.Using(transferCtrDetailDomain))
            {
                var proxies = baseParams.Params.GetAs<Proxy[]>("records");

                var ids = proxies.Select(x => x.Id).ToArray();

                var transferDetails = transferCtrDetailDomain.GetAll()
                    .Where(x => ids.Contains(x.Id))
                    .Fetch(x => x.TransferCtr)
                    .ThenFetch(x => x.ObjectCr)
                    .ThenFetch(x => x.RealityObject)
                    .ToDictionary(x => x.Id);

                if (transferDetails.Count != proxies.Length)
                {
                    throw new ArgumentException("records contains item with invalid TransferCtrDetailId");
                }

                var records = proxies
                    .Select(x => new ByRefundTransferCtrRecord
                    {
                        Sum = x.Sum,
                        TransferCtrDetail = transferDetails.Get(x.Id)
                    })
                    .ToArray();

                return new DistributionByRefundTransferCtrArgs(distributable, records);
            }
        }

        public static DistributionByRefundTransferCtrArgs FromManyParams(BaseParams baseParams, int counter, decimal distribSum)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");

            var distributable = DistributionHelper.ExtractDistributable(baseParams, counter);

            var container = ApplicationContext.Current.Container;
            var transferCtrDetailDomain = container.ResolveDomain<TransferCtrPaymentDetail>();

            using (container.Using(transferCtrDetailDomain))
            {
                var proxies = baseParams.Params.GetAs<Proxy[]>("records");

                var ids = proxies.Select(x => x.Id).ToArray();

                var transferDetails = transferCtrDetailDomain.GetAll()
                    .Where(x => ids.Contains(x.Id))
                    .Fetch(x => x.TransferCtr)
                    .ThenFetch(x => x.ObjectCr)
                    .ThenFetch(x => x.RealityObject)
                    .ToDictionary(x => x.Id);

                if (transferDetails.Count != proxies.Length)
                {
                    throw new ArgumentException("records contains item with invalid TransferCtrDetailId");
                }

                var records = proxies
                    .Select(x => new ByRefundTransferCtrRecord
                    {
                        Sum = x.Sum,
                        TransferCtrDetail = transferDetails.Get(x.Id)
                    })
                    .ToArray();

                return new DistributionByRefundTransferCtrArgs(distributable, records, distribSum);
            }
        }

        private class Proxy
        {
            public long Id { get; set; }

            public decimal Sum { get; set; }
        }
    }

    public sealed class ByRefundTransferCtrRecord
    {
        public TransferCtrPaymentDetail TransferCtrDetail { get; set; }

        public decimal Sum { get; set; }
    }
}