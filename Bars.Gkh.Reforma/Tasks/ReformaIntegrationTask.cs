namespace Bars.Gkh.Reforma.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Domain.Impl;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.ChangeTracker;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Impl;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.Performer;
    using Bars.Gkh.Reforma.PerformerActions.GetCompanyProfile;
    using Bars.Gkh.Reforma.PerformerActions.GetHouseInfo;
    using Bars.Gkh.Reforma.PerformerActions.GetHouseList;
    using Bars.Gkh.Reforma.PerformerActions.GetReportingPeriodList;
    using Bars.Gkh.Reforma.PerformerActions.GetRequestList;
    using Bars.Gkh.Reforma.PerformerActions.SetCompanyProfile;
    using Bars.Gkh.Reforma.PerformerActions.SetHouseLinkToOrganization;
    using Bars.Gkh.Reforma.PerformerActions.SetHouseProfile;
    using Bars.Gkh.Reforma.PerformerActions.SetHouseUnlinkFromOrganization;
    using Bars.Gkh.Reforma.PerformerActions.SetNewCompany;
    using Bars.Gkh.Reforma.PerformerActions.SetNewHouse;
    using Bars.Gkh.Reforma.PerformerActions.SetRequestForSubmit;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;

    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;

    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     Таск регулярного выполнения синхронизации с Реформой ЖКХ
    /// </summary>
    public class ReformaIntegrationTask : BaseTask
    {
        private static readonly object syncObject = new object();
        private TypeIntegration typeIntegration;

        /// <summary>
        ///     Признак того, что активна другая интеграция
        /// </summary>
        public static bool IsRunning { get; private set; }

        #region Public Methods and Operators

        /// <summary>
        ///     Исполнение действия
        /// </summary>
        /// <param name="params">
        ///     Параметры
        /// </param>
        public override void Execute(DynamicDictionary @params)
        {
            var enabled = false;
            this.Container.UsingForResolved<ISyncService>((container, service) => enabled = (bool)((Dictionary<string, object>)service.GetParams().Data)["Enabled"]);

            if (!enabled)
            {
                return;
            }

            lock (ReformaIntegrationTask.syncObject)
            {
                if (ReformaIntegrationTask.IsRunning)
                {
                    return;
                }

                ReformaIntegrationTask.IsRunning = true;
            }

            try
            {
                this.typeIntegration = @params.GetAs("typeIntegration", TypeIntegration.Automatic);

                using (this.Container.BeginScope())
                {
                    this.Execute();
                }
            }
            catch (Exception e)
            {
                this.Container.UsingForResolved<ILogger>((container, manager) => manager.LogError(e, e.ToString()));
            }
            finally
            {
                lock (ReformaIntegrationTask.syncObject)
                {
                    ReformaIntegrationTask.IsRunning = false;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Синхронизация УО. Последний шаг: создает новые УО
        /// </summary>
        /// <param name="performer">
        ///     Планировщик действий синхронизации
        /// </param>
        /// <param name="inns">
        ///     ИНН
        /// </param>
        private void CreateNewCompanies(ISyncActionPerformer performer, IEnumerable<string> inns)
        {
            foreach (var inn in inns)
            {
                var currentInn = inn;
                performer.AddToQueue<SetNewCompanyAction>().WithParameters(inn).WithCallback(
                    result =>
                        {
                            if (!result.Success)
                            {
                                return;
                            }

                            var changedMoService = this.Container.ResolveDomain<ChangedManOrg>();
                            var manOrgService = this.Container.Resolve<IManOrgService>();
                            try
                            {
                                changedMoService.Save(new ChangedManOrg { ManagingOrganization = manOrgService.GetManOrgByInn(currentInn) });
                            }
                            finally
                            {
                                this.Container.Release(changedMoService);
                                this.Container.Release(manOrgService);
                            }
                        });
            }
        }

        /// <summary>
        ///     Выполнение действий
        /// </summary>
        private void Execute()
        {
            var argument = new Arguments { { "typeIntegration", this.typeIntegration } };
            var provider = this.Container.Resolve<ISyncProvider>(argument);
            try
            {
                // синхронизация УО
                this.ProcessManOrgs(provider.Performer);
                provider.Performer.Perform();

                // синхронизация периодов
                provider.Performer.AddToQueue<GetReportingPeriodListAction>();
                provider.Performer.Perform();

                var periods = this.GetActivePeriods();
                if (periods.Length > 0)
                {
                    // создаем задачи на создание профилей УО в активных периодах
                    this.ProcessNewCompanyProfiles(provider.Performer, periods);
                    provider.Performer.Perform();

                    // синхронизация изменений профилей УО
                    this.ProcessChangedCompanyProfiles(provider.Performer, periods);
                    provider.Performer.Perform();
                }

                // синхронизация привязок домов к УО
                this.ProcessHouseLinkage(provider.Performer);
                provider.Performer.Perform();

                if (periods.Length > 0)
                {
                    // синхронизация изменений профилей жилых домов
                    this.ProcessChangedHouseProfiles(provider.Performer, periods);
                    provider.Performer.Perform();
                }
            }
            catch (Exception e)
            {
                provider.Logger.SetException(e);
                throw;
            }
            finally
            {
                provider.Close();
                ManOrgService.Clear();
                this.Container.Release(provider);
            }
        }

        private ReportingPeriodDict[] GetActivePeriods()
        {
            var periodService = this.Container.ResolveDomain<ReportingPeriodDict>();
            try
            {
                return periodService.GetAll().Where(x => x.Synchronizing && x.PeriodDi != null).ToArray();
            }
            finally
            {
                this.Container.Release(periodService);
            }
        }

        /// <summary>
        ///     Создает задачу на создание профиля УО за текущие активные периоды
        /// </summary>
        /// <param name="performer">Планировщик действий синхронизации</param>
        /// <param name="periods">Периоды</param>
        private void ProcessNewCompanyProfiles(ISyncActionPerformer performer, ReportingPeriodDict[] periods)
        {
            var manOrgService = this.Container.Resolve<IManOrgService>();

            try
            {
                var inns = manOrgService.GetSynchronizableInns();
                foreach (var period in periods)
                {
                    var currentPeriod = period;

                    foreach (var inn in inns)
                    {
                        var currentInn = inn;
                        var callback = new Action<SyncActionResult>(
                            result =>
                                {
                                    if (result.Success || result.ErrorDetails.Code != ErrorCodes.MissingCompanyProfileInThisReportingPeriod)
                                    {
                                        return;
                                    }

                                    var changesService = this.Container.ResolveDomain<ChangedManOrg>();
                                    try
                                    {
                                        changesService.Save(
                                            new ChangedManOrg { PeriodDi = new PeriodDi { Id = currentPeriod.PeriodDi.Id }, ManagingOrganization = manOrgService.GetManOrgByInn(currentInn) });
                                    }
                                    finally
                                    {
                                        this.Container.Release(changesService);
                                    }
                                });
                        var parameters = new GetCompanyProfileParams { Inn = currentInn, PeriodExternalId = period.ExternalId };

                        if (currentPeriod.Is_988)
                        {
                            performer.AddToQueue<GetCompanyProfile988Action, GetCompanyProfileParams, CompanyProfileData988>().WithParameters(parameters).WithCallback(result => callback(result));
                        }
                        else
                        {
                            performer.AddToQueue<GetCompanyProfileAction, GetCompanyProfileParams, CompanyProfileData>().WithParameters(parameters).WithCallback(result => callback(result));
                        }
                    }
                }
            }
            finally
            {
                this.Container.Release(manOrgService);
            }
        }

        /// <summary>
        ///     Обрабатывает изменения профилей УО
        /// </summary>
        /// <param name="performer">
        ///     Планировщик действий синхронизации
        /// </param>
        /// <param name="periods">Периоды</param>
        private void ProcessChangedCompanyProfiles(ISyncActionPerformer performer, ReportingPeriodDict[] periods)
        {
            var manOrgService = this.Container.Resolve<IManOrgService>();
            var changesService = this.Container.ResolveDomain<ChangedManOrg>();
            try
            {
                this.Container.InTransaction(
                    () =>
                        {
                            var groupedChanges = changesService.GetAll()
                            .Where(x => x.ManagingOrganization.ActivityGroundsTermination == GroundsTermination.NotSet)
                            .AsEnumerable()
                            .GroupBy(x => x.ManagingOrganization.Id)
                            .ToDictionary(x => x.Key, x => x.ToArray());

                            var manOrgsToDelete = changesService.GetAll()
                                .Where(x => x.ManagingOrganization.ActivityGroundsTermination != GroundsTermination.NotSet)
                                .Select(x => x.Id)
                                .ToList();

                            // удаляем УО, синхронизация к-х нецелесообразна
                            foreach (var manOrg in manOrgsToDelete)
                            {
                                changesService.Delete(manOrg);
                            }

                            foreach (var changes in groupedChanges.Values)
                            {
                                var changeIds = changes.Select(x => x.Id).ToArray();
                                var periodIds = periods.Select(x => x.PeriodDi.Id);
                                if (changes.Any(x => x.PeriodDi == null || periodIds.Contains(x.PeriodDi.Id))
                                    && manOrgService.IsSynchronizable(changes[0].ManagingOrganization))
                                {
                                    var actions = new List<IQueuedActionConfigurator>();
                                    foreach (var period in periods)
                                    {
                                        var action = period.Is_988
                                            ? performer.AddToQueue<SetCompanyProfile988Action, SetCompanyProfileParams, object>()
                                            : performer.AddToQueue<SetCompanyProfileAction, SetCompanyProfileParams, object>();

                                        action.WithParameters(
                                            new SetCompanyProfileParams
                                            {
                                                ManagingOrganizationId = changes[0].ManagingOrganization.Id,
                                                PeriodId = period.PeriodDi.Id,
                                                PeriodExternalId = period.ExternalId
                                            });

                                        actions.Add(action);
                                    }

                                    performer.WhenAll(
                                        actions,
                                        results =>
                                        {
                                            this.Container.InTransaction(
                                                () =>
                                                {
                                                    this.Container.UsingForResolved<IDomainService<ChangedManOrg>>(
                                                        (container, service) =>
                                                        {
                                                            foreach (var result in results.Where(x => x.Success).Where(result => result.Data is long))
                                                            {
                                                                groupedChanges[(long) result.Data].Select(x => x.Id).ForEach(x => service.Delete(x));
                                                            }
                                                        });
                                                });
                                        });
                                }
                                else
                                {
                                    foreach (var changeId in changeIds)
                                    {
                                        changesService.Delete(changeId);
                                    }
                                }
                            }
                        });
            }
            finally
            {
                this.Container.Release(manOrgService);
                this.Container.Release(changesService);
            }
        }

        /// <summary>
        ///     Обрабатывает именения профилей жилых домов
        /// </summary>
        /// <param name="performer">
        ///     Планировщик действий синхронизации
        /// </param>
        /// <param name="periods">Периоды</param>
        private void ProcessChangedHouseProfiles(ISyncActionPerformer performer, ReportingPeriodDict[] periods)
        {
            var roService = this.Container.ResolveDomain<RefRealityObject>();
            var changesService = this.Container.ResolveDomain<ChangedRobject>();
            try
            {
                this.Container.InTransaction(
                    () =>
                        {
                            var groupedChanges = changesService.GetAll()
                            .Where(x => x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                            .AsEnumerable()
                            .GroupBy(x => x.RealityObject.Id)
                            .ToDictionary(x => x.Key, x => x.ToArray());

                            // удаляем дома, синхронизация которых нецелесообразна
                            var roToDelete = changesService.GetAll().Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Razed).Select(x => x.Id).ToList();
                            foreach (var robject in roToDelete)
                            {
                                changesService.Delete(robject);

                            }

                            var roPermissionsDict =
                                roService.GetAll()
                                         .Where(x => x.RefManagingOrganization != null)
                                         .Select(x => new { x.RealityObject.Id, x.RefManagingOrganization.RequestStatus })
                                         .AsEnumerable()
                                         .GroupBy(x => x.Id)
                                         .ToDictionary(x => x.Key, x => x.First().RequestStatus);

                            foreach (var roChanges in groupedChanges)
                            {
                                var permission = roPermissionsDict.Get(roChanges.Key);
                                var changeIds = roChanges.Value.Select(x => x.Id).ToArray();
                                var periodIds = periods.Select(x => x.PeriodDi.Id);
                                if (roChanges.Value.Any(x => x.PeriodDi == null || periodIds.Contains(x.PeriodDi.Id)) && permission == RequestStatus.approved)
                                {
                                    var actions = new List<IQueuedActionConfigurator>();
                                    foreach (var period in periods)
                                    {
                                        var action = period.Is_988
                                                         ? performer.AddToQueue<SetHouseProfile988Action, SetHouseProfileParams, object>()
                                                         : performer.AddToQueue<SetHouseProfileAction, SetHouseProfileParams, object>();

                                        action.WithParameters(new SetHouseProfileParams { RobjectId = roChanges.Key, PeriodDiId = period.PeriodDi.Id });

                                        actions.Add(action);
                                    }

                                    performer.WhenAll(
                                        actions,
                                        results =>
                                            {
                                                this.Container.InTransaction(
                                                    () =>
                                                        {
                                                            this.Container.UsingForResolved<IDomainService<ChangedRobject>>(
                                                                (container, service) =>
                                                                    {
                                                                        foreach (var result in results.Where(x => x.Success).Where(result => result.Data is long))
                                                                        {
                                                                            groupedChanges[(long)result.Data].Select(x => x.Id).ForEach(x => service.Delete(x));
                                                                        }
                                                                    });
                                                        });
                                            });
                                }
                                else
                                {
                                    foreach (var changeId in changeIds)
                                    {
                                        changesService.Delete(changeId);
                                    }
                                }
                            }
                        });
            }
            finally
            {
                this.Container.Release(roService);
                this.Container.Release(changesService);
            }
        }

        /// <summary>
        ///     Обрабатывает привязки домов к УО.
        ///     Шаг 1: Попытка связать существующие привязки с жилыми домами в системе
        /// </summary>
        /// <param name="performer">
        ///     Планировщик действий синхронизации
        /// </param>
        private void ProcessHouseLinkage(ISyncActionPerformer performer)
        {
            var manOrgService = this.Container.Resolve<IManOrgService>();
            var fiasService = this.Container.Resolve<IFiasRepository>();
            try
            {
                var regionGuid = fiasService.GetAll().Where(x => x.AOLevel == FiasLevelEnum.Region).Select(x => x.AOGuid).FirstOrDefault();
                var inns = manOrgService.GetSynchronizableInns();
                foreach (var inn in inns)
                {
                    var s = inn;
                    performer.AddToQueue<GetHouseListAction, GetHouseListParams, HouseData[]>().WithParameters(new GetHouseListParams { Inn = inn, RegionGuid = regionGuid }).WithCallback(
                        result =>
                            {
                                if (result.Success)
                                {
                                    this.Container.InTransaction(() => this.ProcessLinkedHouses(performer, s, result.Data));
                                }
                            });
                }
            }
            finally
            {
                this.Container.Release(manOrgService);
                this.Container.Release(fiasService);
            }
        }

        /// <summary>
        ///     Обновляет привязки домов к УО.
        ///     Шаг 2: Прилинковывает не прилинкованные, отлинковывает отлинкованные, создает не созданные
        /// </summary>
        /// <param name="performer">
        ///     Планировщик действий синхронизации
        /// </param>
        /// <param name="inn">
        ///     ИНН
        /// </param>
        /// <param name="housesData">
        ///     Данные по домам
        /// </param>
        private void ProcessLinkedHouses(ISyncActionPerformer performer, string inn, HouseData[] housesData)
        {
            var data = housesData.ToDictionary(x => x.house_id, x => x.full_address);
            var robjectService = this.Container.Resolve<IRobjectService>();
            var refRobjectService = this.Container.ResolveDomain<RefRealityObject>();
            var ownerContractService = this.Container.ResolveDomain<ManOrgContractOwners>();
            var periodDomain = this.Container.ResolveDomain<PeriodDi>();

            try
            {
                // пройдемся по контрактам УО
                var robjectsManagement = robjectService.GetManagingRobjects(inn).GroupBy(x => x.RobjectId).ToDictionary(x => x.Key, x => x.ToArray());
                foreach (var robjectManagement in robjectsManagement)
                {
                    // проверяем, существует ли дом в реформе
                    var refRobjectIds = refRobjectService.GetAll()
                        .Where(x => x.RealityObject.Id == robjectManagement.Key)
                        .Select(x => new
                        {
                            x.ExternalId,
                            x.RealityObject.Id
                        })
                        .ToArray();

                    // и привязан ли он к УО
                    var linked = refRobjectIds.Length > 0 && refRobjectIds.Select(x => x.ExternalId).All(x => data.ContainsKey(x));

                    // берём все идентификаторы домов, которые пришли с реформы
                    var keys = data.Keys.Where(y => refRobjectIds.Select(x => x.ExternalId).Contains(y)).ToArray();

                    // и если хотя бы 1 дом уже в управлении
                    if (keys.Any())
                    {
                        foreach (var refRobjectId in refRobjectIds)
                        {
                            // удаляем те, которые не пришли по этому дому, это дубли
                            if (!keys.Contains(refRobjectId.ExternalId))
                            {
                                refRobjectService.Delete(refRobjectId.ExternalId);
                            }
                        }
                    }
                    // есть ли активный контракт по данному дому на текущий период
                    var manageable = robjectManagement.Value.Any(x => x.GetIsManageable());

                    // если есть
                    if (manageable)
                    {
                        // и дом привязан в реформе
                        if (linked)
                        {
                            // тогда идем дальше
                            continue;
                        }

                        // иначе найдем первый активный контракт
                        var management = robjectManagement.Value.First(x => x.GetIsManageable());
                        
                        string managementReason;
                        // если договор УК с ТСЖ/ЖСК, то это договор передачи управления
                        if (management.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                        {
                            managementReason = "Передача управления";
                        }
                        else
                        {
                            managementReason = management.TypeManagement == TypeManagementManOrg.UK || management.DocumentName.IsEmpty()
                                ? ownerContractService.GetAll().FirstOrDefault(x => management.Id == x.Id).Return(x => x.ContractFoundation.GetDisplayName())
                                : management.DocumentName;
                        }

                        // и запросим инфу по дому у реформы
                        performer.AddToQueue<GetHouseInfoAction, long, HouseInfo[]>().WithParameters(management.RobjectId).WithCallback(
                            result =>
                                {
                                    // если произошла ошибка и это ошибка отличная от "Дом не найден"
                                    if (!result.Success && result.ErrorDetails.Code != ErrorCodes.HouseAddressWasntFound)
                                    {
                                        // тогда уходим
                                        return;
                                    }

                                    // итак, дом не найден
                                    if (result.Data == null)
                                    {
                                        // создадим его
                                        performer.AddToQueue<SetNewHouseAction, long, int>().WithParameters(management.RobjectId).WithCallback(
                                            actionResult =>
                                                {
                                                    // если все ок
                                                    if (actionResult.Success)
                                                    {
                                                        // привяжем его к УО
                                                        performer.AddToQueue<SetHouseLinkToOrganizationAction, SetHouseLinkToOrganizationParams, object>()
                                                                 .WithParameters(
                                                                     new SetHouseLinkToOrganizationParams
                                                                         {
                                                                             ExternalId = actionResult.Data,
                                                                             DateStart = management.DateStart ?? DateTime.Now,
                                                                             Inn = inn,
                                                                             management_reason = managementReason
                                                                         })
                                                                 .WithCallback(
                                                                     syncActionResult =>
                                                                         {
                                                                             // и если опять успех
                                                                             if (syncActionResult.Success)
                                                                             {
                                                                                 // то включим в список домов для которых нужно создать профиль
                                                                                 this.Container.UsingForResolved<IDomainService<ChangedRobject>>(
                                                                                     (container, service) =>
                                                                                     service.Save(new ChangedRobject { RealityObject = new RealityObject { Id = management.RobjectId } }));
                                                                             }
                                                                         });
                                                    }
                                                });
                                    }
                                    else
                                    {
                                        // дом найден. возможно даже не один
                                        // пройдемся по всем
                                        foreach (var houseInfo in result.Data)
                                        {
                                            var currentHouseId = houseInfo.house_id;
                                            var currentManagingInn = houseInfo.inn;
                                            // если дом не привязан ни к какой УО
                                            if (currentManagingInn.IsEmpty())
                                            {
                                                // привязываем его к текущей
                                                performer.AddToQueue<SetHouseLinkToOrganizationAction, SetHouseLinkToOrganizationParams, object>()
                                                         .WithParameters(
                                                             new SetHouseLinkToOrganizationParams
                                                                 {
                                                                     ExternalId = currentHouseId,
                                                                     DateStart = management.DateStart ?? DateTime.Now,
                                                                     Inn = inn,
                                                                     management_reason = managementReason
                                                                 });
                                            }
                                            // если он привязан уже к кому то, но не к текущей
                                            else if (inn != currentManagingInn)
                                            {
                                                var innerManOrgService = this.Container.Resolve<IManOrgService>();
                                                var innerRobjectService = this.Container.Resolve<IRobjectService>();
                                                try
                                                {
                                                    // и мы можем раскрывать инфу по его УО
                                                    if (innerManOrgService.GetRequestState(currentManagingInn) != RequestStatus.approved)
                                                    {
                                                        continue;
                                                    }

                                                    // находим контракт
                                                    var history = innerRobjectService.GetManagingHistory(management.RobjectId);
                                                    var oldManagement = history.LastOrDefault(x => x.ManOrgInn == currentManagingInn);
                                                    // в теории, двух активных контрактов быть не может. на практике - может
                                                    // выбираем более поздний контракт
                                                    if (oldManagement != null && oldManagement.IsManageable && oldManagement.DateStart >= management.DateStart)
                                                    {
                                                        continue;
                                                    }

                                                    // отвязываем от текущей УО
                                                    performer.AddToQueue<SetUnlinkFromOrganizationAction, SetUnlinkFromOrganizationParams, object>()
                                                             .WithParameters(
                                                                 new SetUnlinkFromOrganizationParams
                                                                     {
                                                                         ExternalId = currentHouseId,
                                                                         DateEnd = oldManagement.Return(x => x.DateEnd) ?? DateTime.Now.AddDays(-1),
                                                                         Reason = oldManagement.Return(x => x.TerminateReason),
                                                                         ReasonType =
                                                                             oldManagement.Return(x => x.ContractStopReason)
                                                                                          .Return(x => x == 0 ? ContractStopReasonEnum.finished_contract : x)
                                                                     })
                                                             .WithCallback(
                                                                 actionResult =>
                                                                     {
                                                                         // если успех
                                                                         if (actionResult.Success)
                                                                         {
                                                                             // привязываем дом к новой УО
                                                                             performer.AddToQueue<SetHouseLinkToOrganizationAction, SetHouseLinkToOrganizationParams, object>()
                                                                                      .WithParameters(
                                                                                          new SetHouseLinkToOrganizationParams
                                                                                              {
                                                                                                  ExternalId = currentHouseId,
                                                                                                  DateStart = management.DateStart ?? DateTime.Now,
                                                                                                  Inn = inn,
                                                                                                  management_reason = managementReason
                                                                                              });
                                                                         }
                                                                         // если не успех и ошибка о том, что не заполнена дата начала управления
                                                                         else if (actionResult.ErrorDetails.Code == ErrorCodes.MissingManagementDate)
                                                                         {
                                                                             // включим дом в список на обновление профиля. профиль включает поле информации о контракте, там дату
                                                                             // и заполним. наверное
                                                                             this.Container.UsingForResolved<IDomainService<ChangedRobject>>(
                                                                                 (container, service) =>
                                                                                 service.Save(new ChangedRobject { RealityObject = new RealityObject { Id = management.RobjectId } }));
                                                                         }
                                                                     });
                                                }
                                                finally
                                                {
                                                    this.Container.Release(innerManOrgService);
                                                    this.Container.Release(innerRobjectService);
                                                    this.Container.Release(ownerContractService);
                                                }
                                            }
                                        }
                                    }
                                });
                    }
                    // активных контрактов по дому нет
                    else
                    {
                        // ищем привязанные в реформе дома
                        var unlinkableRobjectIds = refRobjectIds.Select(x => x.ExternalId).Where(x => data.ContainsKey(x)).ToArray();
                        // если есть
                        if (unlinkableRobjectIds.Length > 0)
                        {
                            // ищем последний контракт
                            var management = robjectManagement.Value.OrderByDescending(x => x.DateEnd).First();
                            // проходим по каждому дому реформы
                            foreach (var unlinkableRobjectId in unlinkableRobjectIds)
                            {
                                // и отвязываем от УО
                                performer.AddToQueue<SetUnlinkFromOrganizationAction, SetUnlinkFromOrganizationParams, object>()
                                         .WithParameters(
                                             new SetUnlinkFromOrganizationParams
                                                 {
                                                     DateEnd = management.DateEnd ?? DateTime.Now,
                                                     ExternalId = unlinkableRobjectId,
                                                     Reason = management.TerminateReason,
                                                     ReasonType =
                                                         management.Return(x => x.ContractStopReason).Return(x => x == 0 ? ContractStopReasonEnum.finished_contract : x)
                                                 })
                                         .WithCallback(
                                             actionResult =>
                                                 {
                                                     // если ошибка о незаполненной дате начала
                                                     if (!actionResult.Success && actionResult.ErrorDetails.Code == ErrorCodes.MissingManagementDate)
                                                     {
                                                         // ставим дом в список домов для обновления профиля
                                                         this.Container.UsingForResolved<IDomainService<ChangedRobject>>(
                                                             (container, service) => service.Save(new ChangedRobject { RealityObject = new RealityObject { Id = management.RobjectId } }));
                                                     }
                                                 });
                            }
                        }

                        // Пытаемся завершить обслуживание оставшихся домов
                        performer.AddToQueue<GetHouseInfoAction, long, HouseInfo[]>().WithParameters(robjectManagement.Key).WithCallback(new PerformerCallback<HouseInfo[]>(
                            result =>
                            {
                                // если произошла ошибка и это ошибка отличная от "Дом не найден"
                                if (!result.Success)
                                {
                                    // тогда уходим
                                    return;
                                }
                                
                                if (result.Data != null)
                                {
                                    // дом найден. возможно даже не один
                                    // пройдемся по всем
                                    foreach (var houseInfo in result.Data)
                                    {
                                        var currentHouseId = houseInfo.house_id;
                                        var currentManagingInn = houseInfo.inn;
                                        // если дом не привязан ни к какой УО
                                        if (currentManagingInn.IsEmpty() || inn != currentManagingInn)
                                        {
                                            continue;
                                        }
                                        // если он привязан уже к кому то, но не к текущей
                                        
                                        var innerManOrgService = this.Container.Resolve<IManOrgService>();
                                        var innerRobjectService = this.Container.Resolve<IRobjectService>();
                                        try
                                        {
                                            // и мы можем раскрывать инфу по его УО
                                            if (innerManOrgService.GetRequestState(currentManagingInn) != RequestStatus.approved)
                                            {
                                                continue;
                                            }

                                            // находим контракт
                                            var history = innerRobjectService.GetManagingHistory(robjectManagement.Key);
                                            var oldManagement = history.OrderBy(x => x.DateEnd).LastOrDefault(x => x.ManOrgInn == currentManagingInn);
                                            // отвязываем от текущей УО
                                            performer.AddToQueue<SetUnlinkFromOrganizationAction, SetUnlinkFromOrganizationParams, object>()
                                                        .WithParameters(
                                                            new SetUnlinkFromOrganizationParams
                                                            {
                                                                ExternalId = currentHouseId,
                                                                DateEnd = oldManagement.Return(x => x.DateEnd) ?? DateTime.Now.AddDays(-1),
                                                                Reason = oldManagement.Return(x => x.TerminateReason),
                                                                ReasonType =
                                                                        oldManagement.Return(x => x.ContractStopReason)
                                                                                    .Return(x => x == 0 ? ContractStopReasonEnum.finished_contract : x)
                                                            })
                                                         .WithCallback(new PerformerCallback<object>(
                                                             actionResult =>
                                                             {
                                                                 // если ошибка о незаполненной дате начала
                                                                 if (!actionResult.Success &&
                                                                     actionResult.ErrorDetails.Code ==
                                                                     ErrorCodes.MissingManagementDate)
                                                                 {
                                                                     // ставим дом в список домов для обновления профиля
                                                                     this.Container
                                                                         .UsingForResolved
                                                                         <IDomainService<ChangedRobject>>(
                                                                             (container, service) =>
                                                                                 service.Save(new ChangedRobject
                                                                                 {
                                                                                     RealityObject =
                                                                                         new RealityObject
                                                                                         {
                                                                                             Id = robjectManagement.Key
                                                                                         }
                                                                                 }));
                                                                 }
                                                             }));
                                        }
                                        finally
                                        {
                                            this.Container.Release(innerManOrgService);
                                            this.Container.Release(innerRobjectService);
                                            this.Container.Release(ownerContractService);
                                        }
                                        
                                    }
                                }
                            }));
                    }
                }
            }
            finally
            {
                this.Container.Release(robjectService);
                this.Container.Release(refRobjectService);
                this.Container.Release(periodDomain);
            }
        }

        /// <summary>
        ///     Обрабатывает УО
        ///     Шаг 1: обновить статусы заявок
        /// </summary>
        /// <param name="performer">
        ///     Планировщик действий синхронизации
        /// </param>
        private void ProcessManOrgs(ISyncActionPerformer performer)
        {
            performer.AddToQueue<GetRequestListAction>().WithCallback(
                result =>
                    {
                        if (result.Success)
                        {
                            this.ProcessNewManOrgs(performer);
                        }
                    });
        }

        /// <summary>
        ///     Обрабатывает УО
        ///     Шаг 2: подает заявки по новым организациям
        /// </summary>
        /// <param name="performer">
        ///     Планировщик действий синхронизации
        /// </param>
        private void ProcessNewManOrgs(ISyncActionPerformer performer)
        {
            performer.AddToQueue<SetRequestForSubmitAction>().WithCallback(
                result =>
                    {
                        if (!result.Success)
                        {
                            return;
                        }

                        var data = (SetRequestForSubmitInnStatus[])result.Data;
                        this.CreateNewCompanies(performer, data.Where(x => x.status == (int)SetRequestForSubmitInnStatusEnum.Missing_INN).Select(x => x.inn));
                    });
        }

        #endregion
    }
}
