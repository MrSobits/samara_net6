namespace Bars.Gkh.RegOperator.Distribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.Utils.Annotations;

    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.GeneralState;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Castle.Windsor;
    using DomainModelServices;
    using Entities;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// Помощник для распределений
    /// </summary>
    internal static class DistributionHelper
    {
        private static IWindsorContainer Container => ApplicationContext.Current.Container;

        /// <summary>
        /// Извлечь множество распределяемых объектов
        /// </summary>
        /// <returns>Список распределений</returns>
        public static IList<IDistributable> ExtractManyDistributables(BaseParams @params)
        {
            var idsString = @params.Params.GetAs<string>("distributionIds", "");
            List<long> idsList = new List<long>();
            if (idsString.Contains(','))
            {
                idsList = idsString.Split(',').Select(long.Parse).ToList();
            }
            else
            {
                idsList = @params.Params.GetAs<List<long>>("distributionIds", new List<long>());
                if (idsList.Count() == 0)
                {
                    return new List<IDistributable>();
                }
            }

            var distributionSource = @params.Params.GetAs<DistributionSource>("distributionSource");

            return DistributionHelper.GetObjectByType(distributionSource, idsList.ToArray());
        }

        /// <summary>
        /// Извлечь распределение
        /// </summary>
        /// <returns>Распределение</returns>
        public static IDistributable ExtractDistributable(BaseParams baseParams, int counter)
        {
            var distributionId = baseParams.Params.GetAs<long>("distributionId", 0L);
            if (distributionId == 0)
            {
                var idsList = baseParams.Params.GetAs<List<long>>("distributionIds", new List<long>());
                if (idsList == null)
                {
                    var idsString = baseParams.Params.GetAs<string>("distributionIds", "");
                    if (idsString.Contains(","))
                    {
                        idsList = idsString.Split(',').Select(long.Parse).ToList();
                    }
                }
                distributionId = idsList[counter];
            }
            var distributionSource = baseParams.Params.GetAs<DistributionSource>("distributionSource");
            return DistributionHelper.GetObjectByType(distributionSource, new [] {distributionId}).FirstOrDefault();
        }

        /// <summary>
        /// Получить распределяемый объект по типу и идентификатору
        /// </summary>
        /// <returns>Распределяемый объект</returns>
        public static IList<IDistributable> GetObjectByType(DistributionSource source, long[] distributionIds)
        {
            if (distributionIds == null)
            {
                throw new ArgumentException("distributionIds must be passed");
            }

            switch (source)
            {
                case DistributionSource.BankStatement:
                    {
                        var domain = DistributionHelper.Container.ResolveDomain<BankAccountStatement>();
                        try
                        {
                            return domain.GetAll().Where(x => distributionIds.Contains(x.Id)).AsEnumerable().Cast<IDistributable>().ToList();
                        }
                        finally
                        {
                            DistributionHelper.Container.Release(domain);
                        }
                    }
                case DistributionSource.SuspenseAccount:
                    {
                        var domain = DistributionHelper.Container.ResolveDomain<SuspenseAccount>();
                        try
                        {
                            return domain.GetAll().Where(x => distributionIds.Contains(x.Id)).AsEnumerable().Cast<IDistributable>().ToList();
                        }
                        finally
                        {
                            DistributionHelper.Container.Release(domain);
                        }
                    }
                case DistributionSource.SubsidyIncome:
                    {
                        var domain = DistributionHelper.Container.ResolveDomain<SubsidyIncome>();
                        try
                        {
                            return domain.GetAll().Where(x => distributionIds.Contains(x.Id)).AsEnumerable().Cast<IDistributable>().ToList();
                        }
                        finally
                        {
                            DistributionHelper.Container.Release(domain);
                        }
                    }
                default:
                    throw new ArgumentException("source is not defined");
            }
        }

        /// <summary>
        /// Получить распределяемый объект по операции
        /// </summary>
        /// <param name="source">Источник</param>
        /// <param name="operation">Операция</param>
        /// <returns>Распределяемый объект</returns>
        public static IDistributable GetObjectByOperation(MoneyOperation operation)
        {
            ArgumentChecker.NotNull(operation, nameof(operation));

            foreach (DistributionSource source in Enum.GetValues(typeof(DistributionSource)))
            {
                switch (source)
                {
                    case DistributionSource.BankStatement:
                        {
                            var domain = DistributionHelper.Container.ResolveDomain<BankAccountStatement>();
                            try
                            {
                                return domain.GetAll().FirstOrDefault(x => x.TransferGuid == operation.OriginatorGuid);
                            }
                            finally
                            {
                                DistributionHelper.Container.Release(domain);
                            }
                        }
                    case DistributionSource.SuspenseAccount:
                        {
                            var domain = DistributionHelper.Container.ResolveDomain<SuspenseAccount>();
                            try
                            {
                                return domain.GetAll().FirstOrDefault(x => x.TransferGuid == operation.OriginatorGuid);
                            }
                            finally
                            {
                                DistributionHelper.Container.Release(domain);
                            }
                        }
                    case DistributionSource.SubsidyIncome:
                        {
                            var domain = DistributionHelper.Container.ResolveDomain<SubsidyIncome>();
                            try
                            {
                                return domain.GetAll().FirstOrDefault(x => x.TransferGuid == operation.OriginatorGuid);
                            }
                            finally
                            {
                                DistributionHelper.Container.Release(domain);
                            }
                        }
                    default:
                        throw new ArgumentException("source is not defined");
                }
            }

            return null;
        }

        /// <summary>
        /// Обновить меня
        /// </summary>
        /// <param name="distributable">Распределение</param>
        public static void UpdateMe(this IDistributable distributable)
        {
            ArgumentChecker.NotNull(distributable, nameof(distributable));

            var t = distributable.GetType();

            var domainType = typeof (IDomainService<>).MakeGenericType(t);

            var domain = (IDomainService)DistributionHelper.Container.Resolve(domainType);
            try
            {
                domain.Update(distributable);
            }
            finally
            {
                DistributionHelper.Container.Release(domain);
            }
        }

        /// <summary>
        /// Проставить статус распределения в "Ожидание подтверждения" и сохранить
        /// </summary>
        /// <param name="distributable">Распределение</param>
        public static void SetWaitingDistribution(this IDistributable distributable)
        {
            DistributionHelper.ValidateState(distributable);

            if (distributable.DistributeState != DistributionState.NotDistributed
                && distributable.DistributeState != DistributionState.PartiallyDistributed)
            {
                throw new ValidationException("Запись должна быть в статусе \"Не распределен\" или \"Частично распределен\"");
            }

            var oldState = distributable.DistributeState;
            distributable.DistributeState = DistributionState.WaitingDistribution;
            distributable.UpdateMe();

            var stateHistoryManager = DistributionHelper.Container.Resolve<IGeneralStateHistoryManager>();
            stateHistoryManager.CreateStateHistory(distributable,oldState,distributable.DistributeState, false);
        }

        /// <summary>
        /// Проставить статус распределения в "Ожидание отмены" и сохранить
        /// </summary>
        /// <param name="distributable">Распределение</param>
        public static void SetWaitingCanellation(this IDistributable distributable)
        {
            DistributionHelper.ValidateState(distributable);

            if (distributable.DistributeState != DistributionState.Distributed
                && distributable.DistributeState != DistributionState.PartiallyDistributed)
            {
                throw new ValidationException("Запись должна быть в статусе \"Распределен\" или \"Частично распределен\"");
            }

            var oldState = distributable.DistributeState;
            distributable.DistributeState = DistributionState.WaitingCancellation;
            distributable.UpdateMe();

            var stateHistoryManager = DistributionHelper.Container.Resolve<IGeneralStateHistoryManager>();
            stateHistoryManager.CreateStateHistory(distributable, oldState, distributable.DistributeState, false);
        }

        private static void ValidateState(IDistributable distributable)
        {
            if (distributable.DistributeState == DistributionState.WaitingDistribution)
            {
                throw new ValidationException("Запись в процессе распределения");
            }

            if (distributable.DistributeState == DistributionState.WaitingCancellation)
            {
                throw new ValidationException("Запись в процессе отмены распределения");
            }
        }
    }
}