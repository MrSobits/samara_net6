namespace Bars.Gkh.RegOperator.Distribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainEvent.Events;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.FormatDataExport.ProxySelectors.Impl;
    using Bars.Gkh.GeneralState;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.PartialOperationCancellation.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Utils;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Провайдер для работы с банковскими операциями
    /// </summary>
    public class DistributionProviderImpl : IDistributionProvider
    {
        private static readonly object syncRoot = new object();

        private readonly IWindsorContainer container;
        private readonly IUserIdentity userIdentity;
        private readonly IAuthorizationService authService;
        private readonly IRealtyObjectPaymentSession session;
        private readonly ISessionProvider sessionProvider;
        private readonly IPersonalAccountRecalcEventManager recalcManager;
        private readonly IGeneralStateHistoryManager stateHistoryManager;

        private readonly IGeneralStateHistoryService stateHistoryService;

        private readonly ITransferRepository transferRepo;
        private readonly IDomainService<DistributionDetail> detailDomain;
        private readonly IFileManager fileManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="userIdentity">Интерфейс идентификатора пользователя</param>
        /// <param name="authService">Интерфейс сервиса авторизации.</param>
        /// <param name="session">Сессия оплаты</param>
        /// <param name="sessionProvider">Провайдер сессии</param>
        /// <param name="recalcManager">Интерфейс создания отсечек перерасчета для ЛС</param>
        /// <param name="transferRepo">Репозиторий трансферов</param>
        /// <param name="stateHistoryManager">Менеджер создания и сохранения истории изменения стаусов</param>
        /// <param name="stateHistoryService">Сервис создания истории изменения статусов</param>
        /// <param name="detailDomain">Домен-сервис <see cref="DistributionDetail" /></param>
        /// <param name="fileManager">Интерфейс представлющий методы для работы с файлами</param>
        public DistributionProviderImpl(
            IWindsorContainer container,
            IUserIdentity userIdentity,
            IAuthorizationService authService,
            IRealtyObjectPaymentSession session,
            ISessionProvider sessionProvider,
            IPersonalAccountRecalcEventManager recalcManager,
            ITransferRepository transferRepo, 
            IGeneralStateHistoryManager stateHistoryManager,
            IGeneralStateHistoryService stateHistoryService,
            IDomainService<DistributionDetail> detailDomain,
            IFileManager fileManager)
        {
            this.container = container;
            this.userIdentity = userIdentity;
            this.authService = authService;
            this.session = session;
            this.sessionProvider = sessionProvider;
            this.recalcManager = recalcManager;
            this.stateHistoryManager = stateHistoryManager;
            this.stateHistoryService = stateHistoryService;
            this.transferRepo = transferRepo;
            this.detailDomain = detailDomain;
            this.fileManager = fileManager;
        }

        public ITransferRepository<PersonalAccountPaymentTransfer> persAccPaymentTranfserRepo { get; set; }

        public ITransferRepository<RealityObjectTransfer> realityObjectTranfserRepo { get; set; }

        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Получить список всех распределений
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult List(BaseParams baseParams)
        {
            var permissionNs = baseParams.Params.GetAs<string>("permissionNs", ignoreCase: true);
            var isForDetailWindow = baseParams.Params.GetAs<bool>("isForDetailWindow");
            var isForPaymentsWindow = baseParams.Params.GetAs<bool>("isForPaymentsWindow");
            var distributables = isForPaymentsWindow ? null : DistributionProviderImpl.GetDistributables(baseParams);

      

            var data = this.container.ResolveAll<IDistribution>()
                .WhereIf(!isForPaymentsWindow && distributables.IsNotEmpty(), x => distributables.All(x.CanApply))
                .WhereIf(!isForPaymentsWindow && !isForDetailWindow && permissionNs.IsEmpty(),
                    x => this.authService.Grant(this.userIdentity, x.PermissionId))
                .WhereIf(!isForPaymentsWindow && !isForDetailWindow && permissionNs.IsNotEmpty(),
                    x => this.authService.Grant(this.userIdentity, x.GetPermissionId(permissionNs)))
                .Select(x => new
                {
                    x.Name,
                    x.Code,
                    x.Route,
                    x.DistributableAutomatically
                })
                .ToArray();

            return new ListDataResult(data, data.Length);
        }

        /// <summary>
        /// Получить список  распределений субсидий
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListSubsidyDistribution(BaseParams baseParams)
        {
            var data = this.container.ResolveAll<IDistribution>()
                .Where(x => x.Code.Contains("Subsidy"))
                .Select(x => new
                {
                    x.Name,
                    x.Code,
                    x.Route
                })
                .ToArray();

            return new ListDataResult(data, data.Length);
        }

        /// <summary>
        /// Проверить возможность распределения
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult Validate(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("code");

            var validators = this.container.ResolveAll<IDistributionValidator>();

            using (this.container.Using(validators))
            {
                foreach (var validator in validators.Where(x => x.Code == code && x.IsMandatory && !x.IsApply))
                {
                    var result = validator.Validate(baseParams);

                    if (!result.Success)
                    {
                        return result;
                    }
                }
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult SoftValidate(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("code");

            var validators = this.container.ResolveAll<IDistributionValidator>();
            var messages = new List<string>();

            using (this.container.Using(validators))
            {
                foreach (var validator in validators.Where(x => x.Code == code && !x.IsMandatory && !x.IsApply))
                {
                    var result = validator.Validate(baseParams);

                    // в отличии от обычной проверки, здесь мы собираем все ошибки и даём право пользователю решить
                    // продолжать работу по распределению или нет
                    if (!result.Success)
                    {
                        messages.Add(result.Message);
                    }
                }
            }

            return new BaseDataResult(messages.Count == 0, messages.Count > 0 ? messages.AggregateWithSeparator(", ").Append(".") : null);
        }

        public IDataResult SoftApplyValidate(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("code");

            var validators = this.container.ResolveAll<IDistributionValidator>();
            var messages = new List<string>();

            using (this.container.Using(validators))
            {
                foreach (var validator in validators.Where(x => x.Code == code && !x.IsMandatory && x.IsApply))
                {
                    var result = validator.Validate(baseParams);

                    // в отличии от обычной проверки, здесь мы собираем все ошибки и даём право пользователю решить
                    // продолжать работу по распределению или нет
                    if (!result.Success)
                    {
                        messages.Add(result.Message);
                    }
                }
            }

            return new BaseDataResult(messages.Count == 0, messages.Count > 0 ? messages.AggregateWithSeparator(", ").Append(".") : null);
        }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        public IDataResult Apply(BaseParams baseParams)
        {
            return this.InTryCatch(() => this.ApplyInternal(baseParams));
        }

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        public IDataResult Undo(BaseParams baseParams)
        {
            return this.InTryCatch(
                () =>
                {
                    var errors = this.UndoInternal(DistributionProviderImpl.GetDistributables(baseParams));

                    return new BaseDataResult(errors.IsEmpty(), errors);
                });
        }

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        public IDataResult UndoPartially(BaseParams baseParams)
        {
            var detailIds = baseParams.Params.GetAs("detailIds", new long[0]);

            return this.InTryCatch(
                () =>
                {
                    var errors = this.UndoInternalPartially(DistributionProviderImpl.GetDistributables(baseParams), detailIds);

                    return new BaseDataResult(errors.IsEmpty(), errors);
                });
        }

        /// <summary>
        /// Отменить зачисление
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        public IDataResult UndoCheckin(BaseParams baseParams)
        {
            return this.InTryCatch(
                () =>
                {
                    var errors = this.UndoCheckinInternal(DistributionProviderImpl.GetDistributables(baseParams));

                    return new BaseDataResult(errors.IsEmpty(), errors);
                });
        }

        /// <summary>
        /// Отменить операцию или зачисление
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult UndoOperationOrUndoCheckin(BaseParams baseParams)
        {
            return this.InTryCatch(
                () =>
                {
                    var errors = new StringBuilder();

                    var distributables = DistributionProviderImpl.GetDistributables(baseParams);

                    errors.Append(this.UndoInternal(distributables.Where(x => x.DistributeState == DistributionState.Distributed
                        || x.DistributeState == DistributionState.PartiallyDistributed)));
                    errors.Append(this.UndoCheckinInternal(distributables));

                    var error = errors.ToString();

                    return new BaseDataResult(error.IsEmpty(), error);
                });
        }

        /// <summary>
        /// Вернуть список объектов распределения
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("code");

            return this.container.Resolve<IDistribution>(code).ListDistributionObjs(baseParams);
        }

        /// <summary>
        /// Вернуть список объектов распределения для автораспределений
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListAutoDistributionObjs(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("code");
            var distribution = this.container.Resolve<IDistribution>(code);
            var distributionArgs = DistributionProviderImpl.GetDistributables(baseParams);

            try
            {
                var autoDistribution = distribution as IDistributionAutomatically;
                if (autoDistribution == null)
                {
                    return BaseDataResult.Error("IDistribution must be convertible to IDistributionAutomatically");
                }

                return autoDistribution.ListAutoDistributionObjs(distributionArgs, baseParams);
            }
            catch (InvalidCastException)
            {
                return BaseDataResult.Error("IDistributable must be convertible to BankAccountStatement");
            }
            finally
            {
                this.container.Release(distribution);
            }
        }

        /// <inheritdoc />
        public IDataResult GetOriginatorName(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("code");
            var distribution = this.container.Resolve<IDistribution>(code);

            using (this.container.Using(distribution))
            {
                return distribution.GetOriginatorName(baseParams);
            }
        }

        /// <inheritdoc />
        public IDataResult CancelOperation(MoneyOperation operation, RepaymentParameters parameter)
        {
            ArgumentChecker.NotNull(operation, nameof(operation));
            ArgumentChecker.NotNull(parameter, nameof(parameter));

            var sumBefore = parameter.UndoTransfers.Sum(x => x.Amount);

            var distributable = DistributionHelper.GetObjectByOperation(operation);
            if (distributable == null)
            {
                throw new ValidationException("Не удалось получить распределяемый объект");
            }

            var distributionOperation = DistributionProviderImpl.GetDistributionOperations(distributable, parameter.OldMoneyOperation).Single();
            distributable.CreateOperation(
                new DynamicDictionary
                {
                    { "distributionCode", distributionOperation.Code },
                    { "operation", operation },
                }, 
                operation.Period);

            var distributionImpl = this.container.Resolve<IDistribution>(distributionOperation.Code.ToString());

            using (this.container.Using(distributionImpl))
            using (var transaction = this.container.Resolve<IDataTransaction>())
            {
                try
                {
                    var oldState = distributable.DistributeState;

                    distributionImpl.Undo(distributable, operation);

                    var ownerInfos = parameter.OldOwners.Select(x => x.GetDescription()).ToArray();
                    var details = this.detailDomain.GetAll()
                        .Where(x => x.EntityId == distributable.Id && x.Source == distributable.Source)
                        .Where(x => ownerInfos.Contains(x.Object))
                        .AsEnumerable()
                        .GroupBy(x => x.Object, x => new DistributionDetailWrapper(distributable, x))
                        .ToDictionary(x => x.Key);

                    var sumAfter = 0m;

                    foreach (var parameterOldOwner in parameter.OldOwners)
                    {
	                    var key = parameterOldOwner.GetDescription();
						var detail = details.Get(key);
                        if (detail != null)
                        {
                            var comparedPayment = PaymentComparator.Compare(
                                parameterOldOwner,
                                detail,
                                parameter.UndoTransfers.Where(x => x.Owner == parameterOldOwner).ToArray(),
                                true);

                            if (comparedPayment.IsNotEmpty() && comparedPayment.Count == detail.Count())
                            {
                                var sum = comparedPayment.Sum(x => x.Key.Sum);
                                var args = DistributionParamsGenerator.GetArgs(
                                    distributable,
                                    distributionImpl,
                                    new Dictionary<ITransferOwner, decimal> { { parameter.OwnerToRepayment, sum } });

                                var result = distributionImpl.Apply(args);
                                if (!result.Success)
                                {
                                    throw new ValidationException(result.Message);
                                }

                                sumAfter += sum;
	                            details.Remove(key);
                            }
                        }
                    }

                    if (sumAfter != sumBefore)
                    {
                        throw new ValidationException("Сумма списания не равна сумме зачисления");
                    }

                    var detailsToDel = this.detailDomain.GetAll()
                        .Where(x => x.EntityId == distributable.Id && x.Source == distributable.Source)
                        .Where(x => ownerInfos.Contains(x.Object))
                        .Select(x => x.Id)
                        .ToList();

                    detailsToDel.ForEach(x => this.detailDomain.Delete(x));

                    distributable.UpdateMe();
                    this.session.Complete();
                    this.recalcManager.SaveEvents();
                    
                    DomainEvents.Raise(new GeneralStateChangeEvent(distributable, oldState, distributable.DistributeState));

                    transaction.Commit();
                }
                catch 
                {
                    this.session.Rollback();
                    transaction.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Извлечь распределяемые объекты
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список объектов</returns>
        internal static IList<IDistributable> GetDistributables(BaseParams baseParams)
        {
            var distributables = DistributionHelper.ExtractManyDistributables(baseParams);
            if (distributables.IsEmpty())
            {
                var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);
                distributables = distributable == null
                    ? new List<IDistributable>(0)
                    : new List<IDistributable> { distributable };
            }

            return distributables;
        }

        private string UndoInternal(IEnumerable<IDistributable> distributables)
        {
            var errors = new StringBuilder();

            foreach (var distributable in distributables)
            {
                if (distributable == null)
                {
                    continue;
                }

                if (distributable.DistributeState != DistributionState.WaitingCancellation
                    && distributable.DistributeState != DistributionState.Distributed
                    && distributable.DistributeState != DistributionState.PartiallyDistributed)
                {
                    errors.AppendFormat(
                        "Id: {0}. Невозможно отменить операцию, находящуюся в статусе \"{1}\"<br>",
                        distributable.Id,
                        distributable.DistributeState.GetEnumMeta().Display);

                    continue;
                }

                var oldState = distributable.RemainSum == 0 ? DistributionState.Distributed : DistributionState.PartiallyDistributed;
                try
                {
                    using (var tr = this.container.Resolve<IDataTransaction>())
                    {
                        var nhSession = this.sessionProvider.GetCurrentSession();
                        var oldFlush = nhSession.FlushMode;
                        nhSession.FlushMode = FlushMode.Never;

                        var operations = this.transferRepo.GetNonCanceledOperations(distributable.TransferGuid).ToArray();
                        var distrOperations = DistributionProviderImpl.GetDistributionOperations(distributable, operations);
                        
                        try
                        {
                            foreach (var distrOperation in distrOperations)
                            {
                                this.container.UsingForResolved<IDistribution>(
                                    distrOperation.Code.ToString(),
                                    (cnt, distribution) => distribution.Undo(distributable, distrOperation.Operation));
                            }

                            this.ClearDetails(distributable);

                            var oldStateWaiting = distributable.DistributeState;
                            distributable.DistributeState = DistributionState.NotDistributed;
                            distributable.DistributionCode = string.Empty;
                            distributable.RemainSum = 0;
                            distributable.DistributionDate = null;
                            distributable.UpdateMe();

                            this.session.Complete();
                            this.recalcManager.SaveEvents();

                            DomainEvents.Raise(new GeneralStateChangeEvent(distributable, oldStateWaiting, distributable.DistributeState));

                            nhSession.FlushMode = oldFlush;

                            tr.Commit();
                        }
                        catch (Exception exception)
                        {
                            this.session.Rollback();
                            tr.Rollback();

                            if (exception.Is<ValidationException>())
                            {
                                errors.AppendFormat("Id: {0}. {1}", distributable.Id, exception.Message).Append("<br>");
                            }

                            throw;
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.UpdoDistributionState(Tuple.Create(distributable, oldState));
                    if (exception.IsNot<ValidationException>())
                    {
                        throw;
                    }
                }
            }

            return errors.ToString();
        }

        private string UndoInternalPartially(IEnumerable<IDistributable> distributables, long[] detailIds)
        {
            var errors = new StringBuilder();
            var transferForSave = new List<Transfer>();

            foreach (var distributable in distributables)
            {
                if (distributable == null)
                {
                    continue;
                }

                if (distributable.DistributeState != DistributionState.WaitingCancellation
                    && distributable.DistributeState != DistributionState.Distributed
                    && distributable.DistributeState != DistributionState.PartiallyDistributed)
                {
                    errors.AppendFormat(
                        "Id: {0}. Невозможно отменить операцию, находящуюся в статусе \"{1}\"<br>",
                        distributable.Id,
                        distributable.DistributeState.GetEnumMeta().Display);

                    continue;
                }

                var details = this.detailDomain.GetAll()
                    .Where(x => x.EntityId == distributable.Id && x.Source == distributable.Source)
                    .Where(x => detailIds.Contains(x.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.Object, x => new DistributionDetailWrapper(distributable, x))
                    .ToDictionary(x => x.Key);

                using (var tr = this.container.Resolve<IDataTransaction>())
                {
                    var operations = this.transferRepo.GetNonCanceledOperations(distributable.TransferGuid).ToArray();
                    var distrOperations = DistributionProviderImpl.GetDistributionOperations(distributable, operations);

                    var allTransfers = this.persAccPaymentTranfserRepo
                        .GetBySourcesGuids(operations.Select(x => x.OriginatorGuid))
                        .Fetch(x => x.Owner)
                        .Cast<Transfer>()
                        .ToList();

                    var realityObjecttransfers = this.realityObjectTranfserRepo
                        .GetBySourcesGuids(operations.Select(x => x.OriginatorGuid))
                        .Fetch(x => x.Owner)
                        .Cast<Transfer>()
                        .ToList();

                    allTransfers.AddRange(realityObjecttransfers);

                    try
                    {
                        foreach (var distrOperation in distrOperations)
                        {
                            var newOperation = distrOperation.Operation.Clone().As<MoneyOperation>();

                            var transfersDict = allTransfers
                                .Where(x => x.Operation.Id == distrOperation.Operation.Id)
                                .GroupBy(x => x.Owner)
                                .ToDictionary(x => x.Key);

                            foreach (var transfersOwner in transfersDict)
                            {
                                var detail = details.Get(transfersOwner.Key.GetDescription());

                                if (detail == null)
                                {
                                    continue;
                                }

                                var comparedPayment = PaymentComparator.Compare(
                                    transfersOwner.Key,
                                    detail,
                                    transfersOwner.Value,
                                    true);

                                foreach (var transfer in comparedPayment.Values.SelectMany(x => x))
                                {
                                    transfer.Operation = newOperation;

                                    transferForSave.Add(transfer);
                                }
                            }

                            this.MoneyOperationDomain.Save(newOperation);
                            transferForSave.ForEach(this.transferRepo.SaveOrUpdate);

                            this.container.UsingForResolved<IDistribution>(
                                distrOperation.Code.ToString(),
                                (cnt, distribution) => distribution.Undo(distributable, newOperation));
                        }

                        var totalCount = this.detailDomain
                            .GetAll()
                            .Count(x => x.EntityId == distributable.Id && x.Source == distributable.Source);

                        this.ClearDetails(distributable, detailIds);

                        var oldStateWaiting = distributable.DistributeState;
                        distributable.DistributeState = totalCount > detailIds.Length
                            ? DistributionState.PartiallyDistributed
                            : DistributionState.NotDistributed;
                        distributable.RemainSum += transferForSave.SafeSum(x => x.Amount);
                        distributable.UpdateMe();

                        this.session.Complete();
                        this.recalcManager.SaveEvents();

                        DomainEvents.Raise(new GeneralStateChangeEvent(distributable, oldStateWaiting, distributable.DistributeState));

                        tr.Commit();
                    }
                    catch (Exception exception)
                    {
                        this.session.Rollback();
                        tr.Rollback();

                        if (exception.Is<ValidationException>())
                        {
                            errors.AppendFormat("Id: {0}. {1}", distributable.Id, exception.Message).Append("<br>");
                        }

                        throw;
                    }
                }
            }

            return errors.ToString();
        }

        private string UndoCheckinInternal(IEnumerable<IDistributable> distributables)
        {
            var errors = new StringBuilder();

            var canDelete = distributables.Where(x => x.DistributeState == DistributionState.NotDistributed);

            using (var tr = this.container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var distributable in canDelete)
                    {
                        var oldState = distributable.DistributeState;

                        distributable.DistributeState = DistributionState.Deleted;
                        distributable.UpdateMe();

                        this.stateHistoryManager.CreateStateHistory(distributable, oldState, distributable.DistributeState, false);
                    }

                    this.session.Complete();
                    this.recalcManager.SaveEvents();
                    this.stateHistoryManager.SaveStateHistories();

                    tr.Commit();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    errors.Append(e.Message).Append("<br>");
                }
                catch (Exception)
                {
                    this.session.Rollback();

                    tr.Rollback();
                    throw;
                }
            }

            return errors.ToString();
        }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        protected virtual IDataResult ApplyInternal(BaseParams baseParams)
        {
            var lockTaken = false;

            // пробуем заблокировать syncRoot в течение 5 минут, в противном случае говорим, что распределение уже выполняется
            Monitor.TryEnter(DistributionProviderImpl.syncRoot, TimeSpan.FromMinutes(5), ref lockTaken);
            if (!lockTaken)
            {
                return new BaseDataResult(false, "Распределение уже выполняется");
            }

            Tuple<IDistributable, DistributionState>[] distributableDict = null;
            try
            {
                var code = baseParams.Params.GetAs<string>("code");
                var distribSum = baseParams.Params.GetAs<decimal>("distribSum");
                var document = baseParams.Files.ContainsKey("Document") ? baseParams.Files["Document"] : null;

                FileInfo fileInfo = null;

                if (document != null)
                {
                    fileInfo = this.fileManager.SaveFile(document);
                }

                if (code.IsEmpty())
                {
                    return new BaseDataResult(false, "Не указан код распределения");
                }

                var distributables = DistributionProviderImpl.GetDistributables(baseParams);
                distributableDict = distributables.Select(
                    x => Tuple.Create(
                        x,
                        x.RemainSum == x.Sum || x.RemainSum == 0 // когда выписка полностью отменена, то Remain = 0
                            ? DistributionState.NotDistributed
                            : DistributionState.PartiallyDistributed))
                    .ToArray();

                if (distributables.IsEmpty())
                {
                    return new BaseDataResult(false, "Не удалось получить ни один распределяемый объект");
                }

                if (
                    distributables.Any(
                        x => x.DistributeState != DistributionState.WaitingDistribution 
                            && x.DistributeState != DistributionState.NotDistributed 
                            && x.DistributeState != DistributionState.PartiallyDistributed))
                {
                    return new BaseDataResult(false,
                        "Запись должна быть в статусе \"Не распределен\" или \"Частично распределен\"");
                }

                var distribution = this.container.Resolve<IDistribution>(code);

                IList<IDataResult> results = new List<IDataResult>();

                using (var tr = this.container.Resolve<IDataTransaction>())
                {
                    var nhSession = this.sessionProvider.GetCurrentSession();
                    var oldFlush = nhSession.FlushMode;
                    nhSession.FlushMode = FlushMode.Never;
                    int counter = 0;
                    var allDistrSum = distributables.SafeSum(x => x.Sum);
                    decimal thisOneDistribSum = 0;

                    try
                    {
                        foreach (var distributable in distributables)
                        {
                            if (distributable.RemainSum == 0)
                            {
                                distributable.RemainSum = distributable.Sum;
                            }

                            thisOneDistribSum = (distributable.Sum / allDistrSum) * distribSum;

                            var args = DistributionProviderImpl.ExtractArgs(distribution, baseParams, distributable, counter, thisOneDistribSum);

                            var oldState = distributable.DistributeState;

                            results.Add(distribution.Apply(args));

                            if (distribution.DistributableAutomatically)
                            {
                                distributable.RemainSum = 0;
                            }
                            else
                            {
                                if (distributables.Count > 1)
                                {
                                    if (allDistrSum == distribSum)
                                    {
                                        distributable.RemainSum = 0;
                                    }
                                    else
                                    {
                                        distributable.RemainSum -= thisOneDistribSum;
                                    }
                                }
                                else
                                {
                                    distributable.RemainSum -= distribSum;
                                }
                            }

                            distributable.DistributeState = distributable.RemainSum == 0
                                ? DistributionState.Distributed
                                : DistributionState.PartiallyDistributed;

                            distributable.DistributionCode = (distributable.DistributionCode ?? string.Empty)
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Union(new[] { code })
                                .ToHashSet()
                                .AggregateWithSeparator(",");

                            distributable.DistributionDate = DateTime.UtcNow;
                            distributable.Document = fileInfo;
                            //проставляем признак РОСП
                            try
                            {
                                if (distributable.Source == DistributionSource.BankStatement)
                                {
                                    var distributionType = baseParams.Params.GetAs<string>("code");
                                    if (distributionType == "TransferCrROSPDistribution")
                                    {
                                        distributable.IsROSP = true;
                                    }
                                }
                              
                            }
                            catch
                            {
                                
                            }
                            distributable.UpdateMe();

                            counter++;

                            this.stateHistoryManager.CreateStateHistory(distributable, oldState, distributable.DistributeState, false);
                        }

                        this.session.Complete();
                        this.recalcManager.SaveEvents();
                        this.stateHistoryManager.SaveStateHistories();

                        nhSession.FlushMode = oldFlush;

                        tr.Commit();
                    }
                    catch
                    {
                        this.session.Rollback();
                        tr.Rollback();
                        throw;
                    }
                }

                if (results.Any(x => !x.Success))
                {
                    return
                        BaseDataResult.Error(
                            $"Не удалось произвести распределение: {string.Join(", ", results.Where(x => !x.Success).Select(x => x.Message))}");
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                this.UpdoDistributionState(distributableDict);
                return new BaseDataResult(false, e.Message);
            }
            catch (Exception)
            {
                this.UpdoDistributionState(distributableDict);
                throw;
            }
            finally
            {
                Monitor.Exit(DistributionProviderImpl.syncRoot);
            }
        }

        private void UpdoDistributionState(params Tuple<IDistributable, DistributionState>[] distributableDict)
        {
            this.sessionProvider.InStatelessTransaction(statelessSession =>
            {
                distributableDict.ForEach(x =>
                {
                    var oldState = x.Item1.DistributeState;
                    x.Item1.DistributeState = x.Item2;
                    statelessSession.InsertOrUpdate(x.Item1);
                    statelessSession.Insert(this.stateHistoryService.CreateStateHistory(x.Item1, oldState, x.Item1.DistributeState));                
                });
            });
            
        }

        internal static IDistributionArgs ExtractArgs(IDistribution distribution, BaseParams baseParams, IDistributable distributable, int counter, decimal thisOneDistribSum)
        {
            if (distribution.DistributableAutomatically)
            {
                return ((IDistributionAutomatically)distribution).GetDistributionArgs(distributable);
            }

            bool isMany = baseParams.Params.GetAs<string>("distributionId", "") == "" ? true : false;
            if (isMany)
            {
                return distribution.ExtractArgsFromMany(baseParams, counter, thisOneDistribSum);
            }
            else
            {
                return distribution.ExtractArgsFrom(baseParams);
            }
        }

        private void ClearDetails(IDistributable distributable, long[] detailIds = null)
        {
            this.detailDomain.GetAll()
                .Where(x => x.EntityId == distributable.Id)
                .Where(x => x.Source == distributable.Source)
                .WhereIf(detailIds != null, x => detailIds.Contains(x.Id))
                .ForEach(x => this.detailDomain.Delete(x.Id));
        }

        private IDataResult InTryCatch(Func<IDataResult> action)
        {
            try
            {
                return action();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            catch (Exception e)
            {
                var msg = e.InnerException?.Message ?? e.Message;

                return new BaseDataResult(false, msg);
            }
        }

        private static List<DistributionOperation> GetDistributionOperations(IDistributable distributable, params MoneyOperation[] operations)
        {
            var result = new List<DistributionOperation>();
            var distribution = distributable as BankAccountStatement;
            if (distribution.IsNull())
            {
                return result;
            }

            distribution.DistributionOperations.Where(x => operations.Any(y => y == x.Operation)).AddTo(result);

            return result;
        }
    }
}