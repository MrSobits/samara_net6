namespace Bars.GkhCr.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;
    using Gkh.Config;
    using Gkh.ConfigSections.ClaimWork.BuilderContract;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Enums;
    using Gkh.Modules.ClaimWork.Contracts;
    using Gkh.Modules.ClaimWork.DomainService;
    using Gkh.Modules.ClaimWork.DomainService.Impl;
    using Gkh.Modules.ClaimWork.Entities;
    using Gkh.Modules.ClaimWork.Enums;
    using GkhCr.Entities;
    using Entities;
    using Gkh.Modules.ClaimWork.DomainService.Lawsuit;
    using Lawsuit;
    using NHibernate.Linq;

    /// <summary>
    /// Сервис для работы с прензиями "Подрядчики, нарушившие условия договора"
    /// </summary>
    public class BuildContractClaimWorkService : BaseClaimWorkService<BuildContractClaimWork>
    {
        /// <summary>
        /// Тип претензионной работы
        /// </summary>
        public override ClaimWorkTypeBase ClaimWorkTypeBase
        {
            get { return ClaimWorkTypeBase.BuildContract; }
        }

        /// <summary>
        /// Вернуть список оснований
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="isPaging">Пагинация необходима</param>
        /// <param name="totalCount">Общее количество</param>
        /// <returns>Список оснований</returns>
        public override IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var domainService = Container.ResolveDomain<BuildContractClaimWork>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var data = domainService.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.BuildContract.ObjectCr.RealityObject.Municipality.Name,
                        Settlement = x.BuildContract.ObjectCr.RealityObject.MoSettlement.Name,
                        Builder = x.BuildContract.Builder.Contragent.Name,
                        x.BuildContract.Builder.Contragent.Inn,
                        x.BuildContract.DocumentNum,
                        x.BuildContract.DocumentDateFrom,
                        x.BuildContract.DateEndWork,
                        x.CreationType,
                        x.CountDaysDelay,
                        x.State,
                        x.IsDebtPaid,
                        x.DebtPaidDate
                    })
                    .Filter(loadParam, Container);

                totalCount = data.Count();

                if (isPaging)
                {
                    data = data.Order(loadParam).Paging(loadParam);
                }
                else
                {
                    data = data.Order(loadParam);
                }

                return data.ToList();
            }
            finally
            {
                Container.Release(domainService);
            }
        }

        /// <summary>
        /// Обновить статусы всех претензий
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="inTask">Создавать задачу</param>
        /// <returns>Результат работы</returns>
        public override IDataResult UpdateStates(BaseParams baseParams, bool inTask = false)
        {
            var id = baseParams.Params.GetAsId();

            var claimWorkDomain = this.Container.ResolveDomain<BuildContractClaimWork>();
            var documentDomain = this.Container.ResolveDomain<DocumentClw>();
            var configProv = this.Container.Resolve<IGkhConfigProvider>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var stateDomain = this.Container.ResolveDomain<State>();
            var lawsuitDomain = this.Container.ResolveDomain<Lawsuit>();
            var autoSelector = this.Container.Resolve<ILawsuitAutoSelector>(BuildContractLawsuitAutoSelector.Id);

            var config = configProv.Get<BuilderContractClaimWorkConfig>();

            try
            {
                var stateNames = new List<string>
                {
                    ClaimWorkStates.FirstLevelViolations,
                    ClaimWorkStates.ViolationIdentificationActNeeded,
                    ClaimWorkStates.ViolationIdentificationActFormed,
                    ClaimWorkStates.NotificationNeeded,
                    ClaimWorkStates.NotificationFormed, 
                    ClaimWorkStates.PretensionNeeded,
                    ClaimWorkStates.PretensionFormed,
                    ClaimWorkStates.PetitionNeeded,
                    ClaimWorkStates.PetitionFormed
                };

                var entityInfo = stateProvider.GetStatefulEntityInfo(typeof(BuildContractClaimWork));
                var stateList = stateDomain.GetAll().Where(x => x.TypeId == entityInfo.TypeId)
                    .Where(x => stateNames.Contains(x.Name) || x.FinalState)
                    .ToList();

                var finalState = stateList.FirstOrDefault(x => x.FinalState);

                if (finalState == null)
                {
                    return new BaseDataResult(false, "Отсутствует конечный статус");
                }

                foreach (var name in stateNames)
                {
                    if (stateList.All(x => x.Name != name))
                    {
                        return new BaseDataResult(false, $"Отсутствует статус {name}");
                    }
                }

                var states = stateList 
                    .GroupBy(x => x.Name)
                    .ToDictionary(x => x.Key, y => y.First());

                var existDocs = documentDomain.GetAll()
                    .WhereIf(id > 0, x => x.ClaimWork.Id == id)
                    .Select(x => new
                    {
                        x.ClaimWork.Id,
                        x.DocumentType
                    })
                    .ToArray()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.DocumentType).ToArray());

                var helper = this.GetHelper(states, config);
                var lastDate = DateTime.Now;
                var defaultState = states.Get(ClaimWorkStates.FirstLevelViolations);

                var claimWorkToUpd = new List<BuildContractClaimWork>();

                var claimWorkQuery =
                    claimWorkDomain.GetAll()
                        .WhereIf(id > 0, x => x.Id == id)
                        .Where(x => !x.State.FinalState);

                var eliminatedViolationBuildContracts = this.GetEliminatedViolationBuildContracts(claimWorkQuery);

                var lawsuitsToUpdate =
                    lawsuitDomain.GetAll()
                                 .Fetch(x => x.ClaimWork)
                                 .Where(x => claimWorkQuery.Any(z => z.Id == x.ClaimWork.Id))
                                 .ToArray();

                lawsuitsToUpdate.ForEach(x => autoSelector.TrySetAll(x));

                claimWorkQuery
                   .Select(x => new
                   {
                       x.Id,
                       StateId = x.State.Id,
                       x.StartingDate,
                       x.CountDaysDelay,
                       x.IsDebtPaid,
                       x.DebtPaidDate,
                       BuildContractId = x.BuildContract.Id
                   })
                   .ToList()
                   .ForEach(x =>
                   {
                       foreach (var rec in helper)
                       {
                           rec.Date = rec.PeriodType == PeriodKind.Day
                               ? x.StartingDate.ToDateTime().AddDays(rec.Count)
                               : x.StartingDate.ToDateTime().AddMonths(rec.Count);
                       }

                       var state = helper.Where(y => y.Date < lastDate)
                           .OrderByDescending(y => y.Date)
                           .ThenByDescending(y => y.Order)
                           .Select(y =>
                           {
                               var existTypeDocs = existDocs.Get(x.Id);

                               if (existTypeDocs != null && existTypeDocs.Contains(y.DocType))
                               {
                                   return y.CreatedState;
                               }
                               return y.NeedState;
                           })
                           .FirstOrDefault() ?? defaultState;

                       var countDaysDelay = (int) lastDate.Subtract(x.StartingDate.ToDateTime()).TotalDays;

                       var claimWork = claimWorkDomain.Load(x.Id);

                       if (x.IsDebtPaid && x.DebtPaidDate.HasValue && x.DebtPaidDate >= DateTime.Now)
                       {
                           state = finalState;
                       }

                       if (eliminatedViolationBuildContracts.Contains(x.BuildContractId))
                       {
                           state = finalState;
                           claimWork.IsDebtPaid = true;
                           claimWork.DebtPaidDate = claimWork.DebtPaidDate ?? DateTime.Now;
                       }

                       if (x.StateId != state.Id || x.CountDaysDelay != countDaysDelay)
                       {
                           claimWork.State = state;
                           claimWork.CountDaysDelay = countDaysDelay;

                           claimWorkToUpd.Add(claimWork);
                       }
                   });

                TransactionHelper.InsertInManyTransactions(this.Container, lawsuitsToUpdate, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, claimWorkToUpd, 1000, true, true);
            }
            finally
            {
                Container.Release(claimWorkDomain);
                Container.Release(documentDomain);
                Container.Release(stateProvider);
                Container.Release(stateDomain);
                Container.Release(configProv);
                Container.Release(lawsuitDomain);
                Container.Release(autoSelector);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Метод возвращает список неоьбходимых документов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Типы документов</returns>
        public override IEnumerable<ClaimWorkDocumentType> GetNeedDocs(BaseParams baseParams)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            var stateDomain = Container.ResolveDomain<State>();
            var docClwDomain = Container.ResolveDomain<DocumentClw>();
            var buildContractClaimWorkDomain = Container.ResolveDomain<BuildContractClaimWork>();
            var configProv = Container.Resolve<IGkhConfigProvider>();

            var config = configProv.Get<BuilderContractClaimWorkConfig>();

            try
            {
                var result = new List<ClaimWorkDocumentType>();

                var claimWorkId = baseParams.Params.GetAsId("claimWorkId");

                var stateNames = new List<string>
                {
                    ClaimWorkStates.FirstLevelViolations,
                    ClaimWorkStates.ViolationIdentificationActNeeded,
                    ClaimWorkStates.NotificationNeeded, 
                    ClaimWorkStates.PretensionNeeded, 
                    ClaimWorkStates.PetitionNeeded
                };

                var entityInfo = stateProvider.GetStatefulEntityInfo(typeof(BuildContractClaimWork));

                var stateList = stateDomain.GetAll()
                                .Where(x => x.TypeId == entityInfo.TypeId)
                                .Where(x => stateNames.Contains(x.Name))
                                .ToList();

                foreach (var name in stateNames)
                {
                    if (stateList.All(x => x.Name != name))
                    {
                        return result;
                    }
                }

                var claimWork = buildContractClaimWorkDomain.Get(claimWorkId);

                var states = stateList
                    .GroupBy(x => x.Name)
                    .ToDictionary(x => x.Key, y => y.First());

                var helper = GetHelper(states, config);
                var lastDate = DateTime.Now;

                foreach (var rec in helper)
                {
                    rec.Date = rec.PeriodType == PeriodKind.Day
                        ? claimWork.StartingDate.ToDateTime().AddDays(rec.Count)
                        : claimWork.StartingDate.ToDateTime().AddMonths(rec.Count);
                }

                var existDocTypes =
                    docClwDomain.GetAll()
                        .Where(x => x.ClaimWork.Id == claimWork.Id)
                        .Select(x => x.DocumentType)
                        .ToList();

                result = helper
                    .Where(y => y.Date < lastDate)
                    .Where(y => !existDocTypes.Contains(y.DocType))
                    .Select(y => y.DocType)
                    .ToList();

                return result;
            }
            finally
            {
                Container.Release(stateProvider);
                Container.Release(stateDomain);
                Container.Release(buildContractClaimWorkDomain);
                Container.Release(docClwDomain);
                Container.Release(configProv);
            }
        }

        /// <summary>
        /// Массовое создание документов по претензиям
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат работы</returns>
        public override IDataResult MassCreateDocs(BaseParams baseParams)
        {
            var typeDoc = baseParams.Params.GetAs<ClaimWorkDocumentType>("typeDocument");
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

            var contractClaimWorkDomain = Container.ResolveDomain<BuildContractClaimWork>();
            var documentClwDomain = Container.ResolveDomain<DocumentClw>();
            var docRules = Container.ResolveAll<IClaimWorkDocRule>();

            try
            {
                var rule = docRules.FirstOrDefault(x => x.ResultTypeDocument == typeDoc);

                if (rule != null)
                {
                    var states = GetStatesByDocType(typeDoc);

                    var claimWorks = contractClaimWorkDomain.GetAll()
                        .Fetch(x => x.RealityObject)
                        .Where(x => states.Contains(x.State.Name))
                        .Where(x => ids.Contains(x.Id))
                        .Where(x => !documentClwDomain.GetAll()
                            .Any(y => y.ClaimWork.Id == x.Id && y.DocumentType == typeDoc))
                        .ToList();

                    if (claimWorks.Count != ids.Count())
                    {
                        return new BaseDataResult(false, "Статус записи не соответствует статусу задолженности");
                    }

                    rule.CreateDocument(claimWorks);
                }

                var updStateResult = UpdateStates(baseParams);

                return updStateResult.Success ? new BaseDataResult(true, "Документы успешно созданы") : updStateResult;
            }
            finally
            {
                Container.Release(docRules);
                Container.Release(contractClaimWorkDomain);
                Container.Release(documentClwDomain);
            }
        }

        /// <summary>
        /// Вернуть типы документов для создания
        /// </summary>
        /// <returns></returns>
        public override IDataResult GetDocsTypeToCreate()
        {
            var configProv = Container.Resolve<IGkhConfigProvider>();

            try
            {
                var config = configProv.Get<BuilderContractClaimWorkConfig>();

                var result = new List<ClaimWorkDocTypeProxy>();

                var isActViolIdentification = config.ViolationNotification.ViolActFormationKind;

                if (isActViolIdentification == DocumentFormationKind.Form)
                {
                    result.Add(new ClaimWorkDocTypeProxy
                    {
                        Name = ClaimWorkDocumentType.ActViolIdentification.GetEnumMeta().Display,
                        Type = ClaimWorkDocumentType.ActViolIdentification
                    });
                }

                var isFormNotif = config.ViolationNotification.NotifFormationKind;

                if (isFormNotif == DocumentFormationKind.Form)
                {
                    result.Add(new ClaimWorkDocTypeProxy
                    {
                        Name = ClaimWorkDocumentType.Notification.GetEnumMeta().Display,
                        Type = ClaimWorkDocumentType.Notification
                    });
                }

                result.Add(new ClaimWorkDocTypeProxy
                {
                    Name = ClaimWorkDocumentType.Pretension.GetEnumMeta().Display,
                    Type = ClaimWorkDocumentType.Pretension
                });

                result.Add(new ClaimWorkDocTypeProxy
                {
                    Name = ClaimWorkDocumentType.Lawsuit.GetEnumMeta().Display,
                    Type = ClaimWorkDocumentType.Lawsuit
                });

                return new BaseDataResult(result);
            }
            finally
            {
                Container.Release(configProv);
            }
        }

        private List<string> GetStatesByDocType(ClaimWorkDocumentType doctype)
        {
            switch (doctype)
            {
                case ClaimWorkDocumentType.ActViolIdentification:
                    return new List<string>
                        {
                            ClaimWorkStates.ViolationActNeeded,
                            ClaimWorkStates.NotificationNeeded,
                            ClaimWorkStates.PretensionNeeded,
                            ClaimWorkStates.PetitionNeeded
                        };
                case ClaimWorkDocumentType.Notification:
                    return new List<string>
                        {
                            ClaimWorkStates.NotificationNeeded,
                            ClaimWorkStates.PretensionNeeded,
                            ClaimWorkStates.PetitionNeeded
                        };
                case ClaimWorkDocumentType.Pretension:
                    return new List<string>
                        {
                            ClaimWorkStates.PetitionNeeded,
                            ClaimWorkStates.PretensionNeeded
                        };
                case ClaimWorkDocumentType.Lawsuit:
                    return new List<string>
                        {
                           ClaimWorkStates.PetitionNeeded
                        };
            }

            return new List<string>();
        }

        private List<ClaimWorkDocHelper> GetHelper(Dictionary<string, State> states, BuilderContractClaimWorkConfig config)
        {
            var helper = new List<ClaimWorkDocHelper>();

            // Настрока - Требует акта выявления нарушений
            var isActViolIdentification = config.ViolationNotification.ViolActFormationKind;

            if (isActViolIdentification == DocumentFormationKind.Form)
            {
                var actViolIdentificationUnitMeasure = config.ViolationNotification.ViolActPeriodKind;
                var actViolIdentificationDelayDaysCount = config.ViolationNotification.ViolActDelayDaysCount;

                helper.Add(new ClaimWorkDocHelper
                {
                    Order = 1,
                    DocType = ClaimWorkDocumentType.ActViolIdentification,
                    PeriodType = actViolIdentificationUnitMeasure,
                    Count = actViolIdentificationDelayDaysCount,
                    NeedState = states.Get(ClaimWorkStates.ViolationIdentificationActNeeded),
                    CreatedState = states.Get(ClaimWorkStates.ViolationIdentificationActFormed)
                });
            }

            // Настрока - Требует формирование уведомления
            var isFormNotif = config.ViolationNotification.NotifFormationKind;

            if (isFormNotif == DocumentFormationKind.Form)
            {
                var notifFormDelayUnitMeasure = config.ViolationNotification.NotifFormPeriodKind;
                var notifFormDelayDaysCount = config.ViolationNotification.NotifFormDelayDaysCount;

                helper.Add(new ClaimWorkDocHelper
                {
                    Order = 2,
                    DocType = ClaimWorkDocumentType.Notification,
                    PeriodType = notifFormDelayUnitMeasure,
                    Count = notifFormDelayDaysCount,
                    NeedState = states.Get(ClaimWorkStates.NotificationNeeded),
                    CreatedState = states.Get(ClaimWorkStates.NotificationFormed)
                });
            }

            // Настрока - Требует формирование претензии
            var pretensionDelayUnitMeasure = config.Pretension.PretensionPeriodKind;
            var pretensionDelayDaysCount = config.Pretension.PretensionDelayDaysCount;

            helper.Add(new ClaimWorkDocHelper
            {
                Order = 3,
                DocType = ClaimWorkDocumentType.Pretension,
                PeriodType = pretensionDelayUnitMeasure,
                Count = pretensionDelayDaysCount,
                NeedState = states.Get(ClaimWorkStates.PretensionNeeded),
                CreatedState = states.Get(ClaimWorkStates.PretensionFormed)
            });

            // Настрока - Требует формирование искового заявления
            var petitionDelayUnitMeasure = config.Petition.PetitionPeriodKind;
            var petitionDelayDaysCount = config.Petition.PetitionDelayDaysCount;

            helper.Add(new ClaimWorkDocHelper
            {
                Order = 4,
                DocType = ClaimWorkDocumentType.Lawsuit,
                PeriodType = petitionDelayUnitMeasure,
                Count = petitionDelayDaysCount,
                NeedState = states.Get(ClaimWorkStates.PetitionNeeded),
                CreatedState = states.Get(ClaimWorkStates.PetitionFormed)
            });

            return helper;
        }

        private List<long> GetEliminatedViolationBuildContracts(IQueryable<BuildContractClaimWork> claimWorkQuery)
        {
            var performedActDomain = Container.ResolveDomain<PerformedWorkAct>();
            var buildContractTypeWorkDomain = Container.ResolveDomain<BuildContractTypeWork>();

            try
            {
                var buildContractTypeWorkQuery = buildContractTypeWorkDomain.GetAll()
                    .Where(x => claimWorkQuery.Any(y => y.BuildContract.Id == x.BuildContract.Id));

                var perfActSumByTypeWork = performedActDomain.GetAll()
                    .Where(x => buildContractTypeWorkQuery.Any(y => y.TypeWork.Id == x.TypeWorkCr.Id))
                    .Select(x => new
                    {
                        x.TypeWorkCr.Id,
                        x.Sum
                    })
                    .ToArray()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Sum.ToDecimal()));


                var result = new List<long>();

                buildContractTypeWorkQuery
                    .Select(x => new
                    {
                        x.BuildContract.Id,
                        TypeWorkId = x.TypeWork.Id,
                        x.BuildContract.Sum
                    })
                    .ToArray()
                    .GroupBy(x => x.Id)
                    .ForEach(x =>
                    {
                        var sum = x.Select(y => y.Sum).First();

                        if (sum <= x.Select(y => perfActSumByTypeWork.Get(y.TypeWorkId)).SafeSum(y => y))
                        {
                            result.Add(x.Key);
                        }
                    });

                return result;

            }
            finally
            {
                Container.Release(performedActDomain);
                Container.Release(buildContractTypeWorkDomain);
            }
        }

        private class ClaimWorkDocHelper
        {
            public int Order { get; set; }

            public ClaimWorkDocumentType DocType { get; set; }

            public State NeedState { get; set; }

            public State CreatedState { get; set; }

            public PeriodKind PeriodType { get; set; }

            public int Count { get; set; }

            public DateTime Date { get; set; }
        }

        private class ClaimWorkDocTypeProxy
        {
            public ClaimWorkDocumentType Type { get; set; }

            public string Name { get; set; }
        }
    }   
}