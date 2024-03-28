namespace Bars.Gkh.RegOperator.Distribution.Args
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.IoC;
    using B4.Application;
    using B4.DataAccess;
    using B4.Utils.Annotations;
    using GkhCr.Entities;
    using GkhCr.Enums;
    using Castle.Windsor;
    using DomainModelServices;
    using Enums;
    using Gkh.Domain;
    using NHibernate.Linq;

    /// <summary>
    /// Аргументы распределения на оплату выполненных работ.
    /// </summary>
    public class DistributionByPerformedWorkActsArgs : AbstractDistributionArgs<DistributionByPerformedWorkActsArgs.ByPerformedWorkRecord>
    {
        private static IWindsorContainer Container
        {
            get
            {
                return ApplicationContext.Current.Container;
            }
        }

        public IDistributable Distributable { get; private set; }

        public override IEnumerable<ByPerformedWorkRecord> DistributionRecords { get; protected set; }

        /// <summary>
        /// Сумма распределения (только для множественного распределения для подсчета финальной суммы)
        /// </summary>
        public decimal SumDistribution { get; protected set; }

        public DistributionByPerformedWorkActsArgs(IDistributable distributable, IEnumerable<ByPerformedWorkRecord> distrRecords)
        {
            ArgumentChecker.NotNull(distributable, nameof(distributable));
            ArgumentChecker.NotNull(distrRecords, nameof(distrRecords));

            Distributable = distributable;
            DistributionRecords = distrRecords;
        }

        public DistributionByPerformedWorkActsArgs(IDistributable distributable, IEnumerable<ByPerformedWorkRecord> distrRecords, decimal distrSum)
        {
            ArgumentChecker.NotNull(distributable, nameof(distributable));
            ArgumentChecker.NotNull(distrRecords, nameof(distrRecords));

            Distributable = distributable;
            DistributionRecords = distrRecords;
            SumDistribution = distrSum;
        }

        /// <summary>
        /// Создает экземпляр класса извлекая необходимые данные из параметров,
        /// переданных с клиента.
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие необходимую информацию</param>
        /// <returns></returns>
        public static DistributionByPerformedWorkActsArgs FromParams(BaseParams baseParams)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionId", "distributionId");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");

            var actPaymentRepo = Container.ResolveDomain<PerformedWorkActPayment>();
            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            using (Container.Using(actPaymentRepo))
            {
                var records = baseParams.Params.GetAs<List<Proxy>>("records");
                var actIds = records.Select(x => x.PerformedWorkActPaymentId).ToList();

                var acts = actPaymentRepo.GetAll()
                    .Fetch(x => x.PerformedWorkAct)
                    .ThenFetch(x => x.ObjectCr)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => actIds.Contains(x.Id))
                    .ToList();

                if (acts.Count != records.Count)
                {
                    throw new ArgumentException("records contains item with invalid ActId");
                }

                return new DistributionByPerformedWorkActsArgs(
                    distributable,
                    acts.Join(records,
                        payment => payment.Id,
                        proxy => proxy.PerformedWorkActPaymentId,
                        (act, proxy) => new ByPerformedWorkRecord(act, proxy.Sum, proxy.ActPaymentType)));
            }
        }

        /// <summary>
        /// Создает экземпляр класса извлекая необходимые данные из параметров,
        /// переданных с клиента.
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие необходимую информацию</param>
        /// <returns></returns>
        public static DistributionByPerformedWorkActsArgs FromManyParams(BaseParams baseParams, int counter, decimal distribSum)
        {
            ArgumentChecker.InCollection(baseParams.Params, "records", "records");
            ArgumentChecker.InCollection(baseParams.Params, "distributionSource", "distributionSource");

            var actPaymentRepo = Container.ResolveDomain<PerformedWorkActPayment>();
            var distributable = DistributionHelper.ExtractDistributable(baseParams, counter);

            using (Container.Using(actPaymentRepo))
            {
                var records = baseParams.Params.GetAs<List<Proxy>>("records");
                var actIds = records.Select(x => x.PerformedWorkActPaymentId).ToList();

                var acts = actPaymentRepo.GetAll()
                    .Fetch(x => x.PerformedWorkAct)
                    .ThenFetch(x => x.ObjectCr)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => actIds.Contains(x.Id))
                    .ToList();

                if (acts.Count != records.Count)
                {
                    throw new ArgumentException("records contains item with invalid ActId");
                }

                return new DistributionByPerformedWorkActsArgs(
                    distributable,
                    acts.Join(records,
                        payment => payment.Id,
                        proxy => proxy.PerformedWorkActPaymentId,
                        (act, proxy) => new ByPerformedWorkRecord(act, proxy.Sum, proxy.ActPaymentType)),
                    distribSum);
            }
        }

        /// <summary>
        /// Запись распределения для акта.
        /// </summary>
        public sealed class ByPerformedWorkRecord
        {
            public ByPerformedWorkRecord(PerformedWorkActPayment performedWorkActPaymnent, decimal sum, ActPaymentType actPaymentType)
            {
                PerformedWorkActPayment = performedWorkActPaymnent;
                Sum = sum;
                ActPaymentType = actPaymentType;
            }

            /// <summary>
            /// Оплачиваемый акт
            /// </summary>
            public PerformedWorkActPayment PerformedWorkActPayment { get; private set; }

            /// <summary>
            /// Сумма оплаты
            /// </summary>
            public decimal Sum { get; private set; }

            /// <summary>
            /// Вид оплаты акта
            /// </summary>
            public ActPaymentType ActPaymentType { get; private set; }
        }

        /// <summary>
        /// Класс, определяющий формат распределения который приходит с клиента.
        /// В соответсвии с этим форматом будут извлекаться данные из BaseParams.
        /// </summary>
        private sealed class Proxy
        {
            public long PerformedWorkActPaymentId { get; set; }

            public decimal Sum { get; set; }

            public ActPaymentType ActPaymentType { get; set; }
        }
    }
}
