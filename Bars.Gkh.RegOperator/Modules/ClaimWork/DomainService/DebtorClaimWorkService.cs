namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.TransitionRules;
    using Bars.Gkh.RegOperator.Tasks.Debtors;

    using Domain;

    using States;
    using Entities;
    using Enums;
    using Repositories;

    using NHibernate.Linq;

    public class DebtorClaimWorkService : DebtorClaimWorkService<DebtorClaimWork>
    {

    }

    /// <summary>
    /// Сервис по работе с основаниями претензионно-исковой работы для неплательщиков
    /// </summary>
    public class DebtorClaimWorkService<T> : BaseClaimWorkService<T>
        where T : DebtorClaimWork
    {
        public IClwStateProvider ClaimWorkStateProvider { get; set; }

        public IDebtorClaimWorkUpdateService DebtorClaimWorkUpdateService { get; set; }

        public override ClaimWorkTypeBase ClaimWorkTypeBase => ClaimWorkTypeBase.Debtor;

        public override IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновление текущего состояния претензий
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="inTask">Выполнять в задаче</param>
        /// <returns>Результат запроса</returns>
        public override IDataResult UpdateStates(BaseParams baseParams, bool inTask = false)
        {
            if (inTask)
            {
                var debtorType = DebtorType.NotSet;
                baseParams.Params["debtorType"] = debtorType;
                return this.CreateTask(baseParams);
            }

            return this.DebtorClaimWorkUpdateService.UpdateStates(baseParams);
        }
        /// <summary>
        /// Вернуть список необходимых документов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IEnumerable<ClaimWorkDocumentType> GetNeedDocs(BaseParams baseParams)
        {
            var clwDomain = this.Container.ResolveDomain<DebtorClaimWork>();
            var docClwRepo = this.Container.ResolveRepository<DocumentClw>();
            var stateRepo = this.Container.Resolve<IStateRepository>();
            var courtClaimRepo = this.Container.ResolveRepository<CourtOrderClaim>();
            try
            {
                var claimWorkId = baseParams.Params.GetAsId("claimWorkId");
                var claimWork = clwDomain.Get(claimWorkId);
                this.ClaimWorkStateProvider.InitCache(new [] {claimWorkId});

                return this.ClaimWorkStateProvider.GetAvailableTransitions(
                    claimWork, this.GetNeedDocRules());
            }
            finally
            {
                this.ClaimWorkStateProvider.Clear();
                this.Container.Release(clwDomain);
                this.Container.Release(docClwRepo);
                this.Container.Release(stateRepo);
                this.Container.Release(courtClaimRepo);
            }
        }
        

        /// <summary>
        /// Вернуть список необходимых документов
        /// </summary>
        /// <returns>Результат запроса</returns>
        private List<IClwTransitionRule> GetNeedDocRules()
        {
            return new List<IClwTransitionRule>
            {
                new IncludeHiLevelDocRule(this.Container),
                new IncludeLawsuitRule(this.Container),
                new IncludeCourtOrderClaimRule(this.Container),
                new IncludePretensionRule(this.Container),
                new IncludeNotificationRule(this.Container)
            };
        }

        /// <summary>
        /// Массовое создание документов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IDataResult MassCreateDocs(BaseParams baseParams)
        {
            var docType = baseParams.Params.GetAs<ClaimWorkDocumentType>("typeDocument");
            var debtorType = baseParams.Params.GetAs<DebtorType>("debtorType");
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

            var clwDomain = this.Container.ResolveDomain<DebtorClaimWork>();
            var documentClwDomain = this.Container.ResolveDomain<DocumentClw>();
            var docRules = this.Container.ResolveAll<IClaimWorkDocRule>();

            var docClwRepo = this.Container.ResolveRepository<DocumentClw>();
            var stateRepo = this.Container.Resolve<IStateRepository>();
            var courtClaimRepo = this.Container.ResolveRepository<CourtOrderClaim>();

            try
            {
                var rule = docRules.FirstOrDefault(x => x.ResultTypeDocument == docType);
                var applyableIds = new List<long>();

                if (rule != null)
                {
                    var rules = this.GetNeedDocRules();

                    var claimWorkQuery = clwDomain.GetAll()
                        .Fetch(x => x.RealityObject)
                        .WhereIf(ids.Length > 0, 
                            x => ids.Contains(x.Id))
                        .WhereIf(
                            debtorType != default(DebtorType),
                            x => x.DebtorType == debtorType)
                        .WhereIf(
                            docType == ClaimWorkDocumentType.CourtOrderClaim,
                            x => x.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
                        .Where(
                            x => !documentClwDomain.GetAll()
                                .Any(y => y.ClaimWork.Id == x.Id && y.DocumentType == docType))
                         .OrderBy(x => x.Id);

                    var endIndex = claimWorkQuery.Count();
                    const int step = 5000;

                    var documentsToCreate = new List<DocumentClw>();

                    for (var startIndex = 0; startIndex < endIndex; startIndex += step)
                    {
                        var claimWorkPortion = claimWorkQuery.Skip(startIndex).Take(step)
                            .AsEnumerable();

                        var claimWorkIds = claimWorkPortion.Select(x => x.Id).ToArray();

                        this.ClaimWorkStateProvider.InitCache(claimWorkIds);

                        var claimWorks = claimWorkPortion
                            .Where(x => this.ClaimWorkStateProvider.GetAvailableTransitions(x, rules).Contains(docType))
                            .ToArray();

                        if (claimWorks.Length != 0)
                        {
                            documentsToCreate.AddRange(rule.FormDocument(claimWorks));
                        }
                        applyableIds.AddRange(claimWorks.Select(x => x.Id).ToArray());

                        this.ClaimWorkStateProvider.Clear();
                    }
                    
                    TransactionHelper.InsertInManyTransactions(this.Container, documentsToCreate, 1000, true, true);
                }

                if (applyableIds.Count == 0)
                {
                    return new BaseDataResult(
                        false,
                        "Ни одна запись не удовлетворяет условиям создания выбранного документа");
                }

                var updStateResult = this.DebtorClaimWorkUpdateService.UpdateStates(applyableIds.ToArray());

                return updStateResult.Success
                    ? (ids.Length == applyableIds.Count
                        ? new BaseDataResult(true, "Документы успешно созданы")
                        : new BaseDataResult(false, "Некоторые записи не удовлетворяют условиям создания выбранного документа"))
                    : updStateResult;
            }
            finally
            {
                this.Container.Release(docRules);
                this.Container.Release(clwDomain);
                this.Container.Release(documentClwDomain);
                this.Container.Release(docClwRepo);
                this.Container.Release(stateRepo);
                this.Container.Release(courtClaimRepo);
            }
        }

        /// <summary>
        /// Вернуть типы документов для создания
        /// </summary>
        /// <returns>Список документов</returns>
        public override IDataResult GetDocsTypeToCreate()
        {
            return new BaseDataResult(
                new List<ClaimWorkDocTypeProxy>
                {
                    new ClaimWorkDocTypeProxy(ClaimWorkDocumentType.Notification),
                    new ClaimWorkDocTypeProxy(ClaimWorkDocumentType.Pretension),
                    new ClaimWorkDocTypeProxy(ClaimWorkDocumentType.Lawsuit),
                    new ClaimWorkDocTypeProxy(ClaimWorkDocumentType.CourtOrderClaim)
                });
        }

        protected IDataResult CreateTask(BaseParams baseParams)
        {
            var taskEntryDomain = this.Container.ResolveDomain<TaskEntry>();
            try
            {
                if (taskEntryDomain.GetAll()
                    .Any(x => x.Status != TaskStatus.Error
                        && x.Status != TaskStatus.Succeeded
                        && x.Parent.TaskCode == DebtorsStateTaskProvider.Code))
                {
                    throw new Exception("Обновление статусов неплательщиков уже запущено");
                }

                return this.TaskManager.CreateTasks(new DebtorsStateTaskProvider(), baseParams);
            }
            finally
            {
                this.Container.Release(taskEntryDomain);
            }
        }

        private class ClaimWorkDocTypeProxy
        {
            public ClaimWorkDocTypeProxy(ClaimWorkDocumentType type)
            {
                this.Type = type;
                this.Name = type.GetEnumMeta().Display;
            }

            // ReSharper disable once MemberCanBePrivate.Local
            public ClaimWorkDocumentType Type { get; private set; }

            // ReSharper disable once MemberCanBePrivate.Local
            public string Name { get; private set; }
        }
    }
}