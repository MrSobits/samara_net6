namespace Bars.Gkh.RegOperator.Distribution
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain;
    using Bars.Gkh.GeneralState;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Tasks.Distribution;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для подтверждения/отмены банковских выписок
    /// </summary>
    public class DistributionService : IDistributionService
    {
        public IWindsorContainer Container { get; set; }

        public IDistributionProvider DistributionProvider { get; set; }

        public ITaskManager TaskManager { get; set; }

        public IGeneralStateHistoryManager StateHistoryManager { get; set; }

        /// <inheritdoc />
        public IDataResult Apply(BaseParams baseParams)
        {
            var distributionArgs = DistributionProviderImpl.GetDistributables(baseParams);

            try
            {
                this.Container.InTransaction(
                    () =>
                    {
                        distributionArgs.ForEach(x => x.SetWaitingDistribution());
                        this.StateHistoryManager.SaveStateHistories();
                    });
            }
            catch (ValidationException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }

            if (this.ShouldCreateTask())
            {
                return this.TaskedResult(
                    this.TaskManager.CreateTasks(
                        new DistributionApplyTaskProvider(this.ExtractDescription(baseParams, distributionArgs)), baseParams));
            }

            return this.DistributionProvider.Apply(baseParams);
        }

        /// <inheritdoc />
        public IDataResult Undo(BaseParams baseParams)
        {
            var distributionArgs = DistributionProviderImpl.GetDistributables(baseParams);

            try
            {
                this.Container.InTransaction(
                    () =>
                    {
                        distributionArgs.ForEach(x => x.SetWaitingCanellation());
                        this.StateHistoryManager.SaveStateHistories();
                    });
            }
            catch (ValidationException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }

            if (this.ShouldCreateTask())
            {
                return this.TaskedResult(
                    this.TaskManager.CreateTasks(
                        new DistributionUndoTaskProvider(this.ExtractDescription(baseParams, distributionArgs)), baseParams));
            }

            return this.DistributionProvider.Undo(baseParams);
        }

        public IDataResult UndoPartially(BaseParams baseParams)
        {
            var distributionArgs = DistributionProviderImpl.GetDistributables(baseParams);

            try
            {
                this.Container.InTransaction(
                    () =>
                    {
                        distributionArgs.ForEach(x => x.SetWaitingCanellation());
                        this.StateHistoryManager.SaveStateHistories();
                    });
            }
            catch (ValidationException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }

            if (this.ShouldCreateTask())
            {
                return this.TaskedResult(
                    this.TaskManager.CreateTasks(
                        new DistributionUndoPartiallyTaskProvider(this.ExtractDescription(baseParams, distributionArgs)), baseParams));
            }

            return this.DistributionProvider.UndoPartially(baseParams);
        }

        private string ExtractDescription(BaseParams baseParams, IEnumerable<IDistributable> distributables)
        {
            var code = baseParams.Params.GetAs<string>("code");
            var detailIds = baseParams.Params.GetAs("detailIds", new long[0]);

            // во время отмены банковских выписок мы не знаем вида распределения
            // поэтому смотрим на детализацию
            if (code.IsEmpty())
            {
                var distribututionDetailDomain = this.Container.ResolveDomain<DistributionDetail>();
                var distributableIds = distributables.Select(x => x.Id).ToArray();

                using (this.Container.Using(distribututionDetailDomain))
                {
                    var detailsCount = distribututionDetailDomain.GetAll()
                        .WhereContains(x => x.EntityId, distributableIds)
                        .WhereIfContains(detailIds.Length > 0, x => x.Id, detailIds)
                        .GroupBy(x => x.EntityId)
                        .Select(x => new
                        {
                            x.Key,
                            Count = x.Count()
                        })
                        .ToDictionary(x => x.Key, x => x.Count);

                    return string.Join("; ", distributables.Select(x => this.GetDesription(detailsCount, x)));
                }
            }

            var distribution = this.Container.Resolve<IDistribution>(code);

            using (this.Container.Using(distribution))
            {
                return string.Join("; ", distributables.Select(x => this.GetDesription(distribution, baseParams, x)));
            }
        }

        private string GetDesription(IDictionary<long, int> detailsCount, IDistributable distributable)
        {
            var prefix = "Объектов распределения";

            var docNum = (distributable as BankAccountStatement)?.DocumentNum;
            var docInfo = docNum.IsEmpty()
                ? string.Empty
                : $", номер документа: {docNum}";

            return $"{prefix}: {detailsCount.Get(distributable.Id)}{docInfo}";
        }

        private string GetDesription(IDistribution distribution, BaseParams baseParams, IDistributable distributable)
        {
            var args = DistributionProviderImpl.ExtractArgs(distribution, baseParams, distributable, 0, 0);
            var prefix = "Объектов распределения";

            if (args is DistributionByAccountsArgs)
            {
                prefix = "Лицевые счета";
            }

            if (args is DistributionByPerformedWorkActsArgs)
            {
                prefix = "Акты выполненных работ";
            }

            if (args is DistributionByRealtyAccountArgs)
            {
                prefix = "Дома";
            }

            if (args is DistributionByRefundTransferCtrArgs)
            {
                prefix = "Заявки на перечисление средств подрядчикам";
            }

            if (args is DistributionByTransferCtrArgs)
            {
                prefix = "Заявки на перечисление средств подрядчикам";
            }

            var docNum = (distributable as BankAccountStatement)?.DocumentNum;
            var docInfo = docNum.IsEmpty()
                ? string.Empty
                : $", номер документа: {docNum}";
            
            return $"{prefix}: {args.DistributionRecords.Cast<object>().Count()}{docInfo}";
        }

        private bool ShouldCreateTask()
        {
            var configProvider = this.Container.GetGkhConfig<RegOperatorConfig>();

            using (this.Container.Using(configProvider))
            {
                return configProvider.GeneralConfig.DistributeStatementsOnExecutor;
            }
        }

        private IDataResult TaskedResult(IDataResult<CreateTasksResult> taskResult)
        {
            if (taskResult.Success)
            {
                return new BaseDataResult(true, "Задача успешно поставлена в очередь на обработку. "
                    + "Информация о статусе распределения содержится в пункте меню \"Задачи\"");
            }

            return taskResult;
        }
    }
}