namespace Bars.Gkh.Reforma.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Domain.Impl;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.ChangeTracker;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Impl;
    using Bars.Gkh.Reforma.Interface.Performer;
    using Bars.Gkh.Reforma.PerformerActions.GetHouseInfo;
    using Bars.Gkh.Reforma.PerformerActions.GetHouseList;
    using Bars.Gkh.Reforma.PerformerActions.SetCompanyProfile;
    using Bars.Gkh.Reforma.PerformerActions.SetHouseLinkToOrganization;
    using Bars.Gkh.Reforma.PerformerActions.SetHouseUnlinkFromOrganization;
    using Bars.Gkh.Reforma.PerformerActions.SetNewHouse;
    using Bars.Gkh.Reforma.ReformaService;

    /// <summary>
    /// Задача ручного обновления информации об УО
    /// </summary>
    public class SetCompanyProfile988Task : BaseManualIntegrationTask<SetCompanyProfileParams>
    {
        private ManOrgService manorgService = ManOrgService.Instance;

        /// <summary>
        /// Домен-сервис УО
        /// </summary>
        public IDomainService<ManagingOrganization> ManagingOrganizationDomain { get; set; }

        /// <summary>
        /// Выполяемое действие
        /// </summary>
        protected override void Execute()
        {
            var managinOrganization = this.ManagingOrganizationDomain.Get(this.TaskParams.ManagingOrganizationId);

            if (this.manorgService.IsSynchronizable(managinOrganization))
            {
                this.SetCompanyProfile();
                this.ProcessHouseLinkage(managinOrganization.Contragent.Inn);
            }

            this.Performer.Perform();
        }

        private void SetCompanyProfile()
        {
            var action = this.Performer.AddToQueue<SetCompanyProfile988Action, SetCompanyProfileParams, object>()
                .WithParameters(this.TaskParams);

            // чистим за собой в таблице изменения по указанной УО
            this.Performer.WhenAll(new List<IQueuedActionConfigurator> { action },
                results =>
                {
                    this.Container.InTransaction(
                        () =>
                        {
                            this.Container.UsingForResolved<IDomainService<ChangedManOrg>>(
                                (container, service) =>
                                {
                                    if (results.Any(x => !x.Success))
                                    {
                                        return;
                                    }

                                    service.GetAll()
                                        .Where(x => x.ManagingOrganization.Id == this.TaskParams.ManagingOrganizationId && x.PeriodDi.Id == this.TaskParams.PeriodId)
                                        .ForEach(x => service.Delete(x.Id));
                                });
                        });
                });
        }

        /// <summary>
        ///     Обрабатывает привязки домов к УО.
        ///     Шаг 1: Попытка связать существующие привязки с жилыми домами в системе
        /// </summary>
        private void ProcessHouseLinkage(string inn)
        {
            var fiasService = this.Container.Resolve<IFiasRepository>();
            var regionGuid = fiasService.GetAll().Where(x => x.AOLevel == FiasLevelEnum.Region).Select(x => x.AOGuid).FirstOrDefault();

            try
            {
                this.Performer.AddToQueue<GetHouseListAction, GetHouseListParams, HouseData[]>().WithParameters(new GetHouseListParams { Inn = inn, RegionGuid = regionGuid }).WithCallback(
                        result =>
                        {
                            if (result.Success)
                            {
                                this.Container.InTransaction(() => this.ProcessLinkedHouses(inn, result.Data));
                            }
                        });
            }
            finally
            {
                this.Container.Release(fiasService);
            }

            
        }

        /// <summary>
        ///     Обновляет привязки домов к УО.
        ///     Шаг 2: Прилинковывает не прилинкованные, отлинковывает отлинкованные, создает не созданные
        /// </summary>
        /// <param name="inn">
        ///     ИНН
        /// </param>
        /// <param name="housesData">
        ///     Данные по домам
        /// </param>
        private void ProcessLinkedHouses(string inn, HouseData[] housesData)
        {
            var data = housesData.ToDictionary(x => x.house_id, x => x.full_address);
            var robjectService = this.Container.Resolve<IRobjectService>();
            var refRobjectService = this.Container.ResolveDomain<RefRealityObject>();
            var ownerContractService = this.Container.ResolveDomain<ManOrgContractOwners>();
            var reportingPeriodDomain = this.Container.ResolveDomain<ReportingPeriodDict>();

            try
            {
                var period = reportingPeriodDomain.GetAll().First(x => x.PeriodDi.Id == this.TaskParams.PeriodId);

                // пройдемся по действующим на период контрактам УО
                var robjectsManagement = robjectService.GetManagingRobjects(inn)
                    .GroupBy(x => x.RobjectId)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                foreach (var robjectManagement in robjectsManagement)
                {
                    // проверяем, существует ли дом в реформе
                    var refRobjectIds =
                        refRobjectService.GetAll().Where(x => x.RealityObject.Id == robjectManagement.Key).Select(x => x.ExternalId).ToArray();

                    // и привязан ли он к УО
                    var linked = refRobjectIds.Length > 0 && refRobjectIds.All(x => data.ContainsKey(x));

                    // есть ли активный контракт по данному дому
                    var manageable = robjectManagement.Value.Any(x => x.GetIsManageable(period.PeriodDi));

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
                        var management = robjectManagement.Value.First(x => x.GetIsManageable(period.PeriodDi));

                        string managementReason;

                        // если договор УК с ТСЖ/ЖСК, то это договор передачи управления
                        if (management.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                        {
                            managementReason = "Передача управления";
                        }
                        else
                        {
                            managementReason = management.TypeManagement == TypeManagementManOrg.UK || management.DocumentName.IsEmpty()
                                ? ownerContractService.GetAll()
                                    .FirstOrDefault(x => management.Id == x.Id)
                                    .Return(x => x.ContractFoundation.GetDisplayName())
                                : management.DocumentName;
                        }

                        // и запросим инфу по дому у реформы
                        this.Performer.AddToQueue<GetHouseInfoAction, long, HouseInfo[]>().WithParameters(management.RobjectId).WithCallback(
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
                                    this.Performer.AddToQueue<SetNewHouseAction, long, int>().WithParameters(management.RobjectId).WithCallback(
                                        actionResult =>
                                        {
                                            // если все ок
                                            if (actionResult.Success)
                                            {
                                                // привяжем его к УО
                                                this.Performer.AddToQueue<SetHouseLinkToOrganizationAction, SetHouseLinkToOrganizationParams, object>()
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
                                                                        service.Save(
                                                                            new ChangedRobject
                                                                            {
                                                                                RealityObject = new RealityObject { Id = management.RobjectId }
                                                                            }));
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
                                            this.Performer.AddToQueue<SetHouseLinkToOrganizationAction, SetHouseLinkToOrganizationParams, object>()
                                                .WithParameters(
                                                    new SetHouseLinkToOrganizationParams
                                                    {
                                                        ExternalId = currentHouseId,
                                                        DateStart = management.DateStart ?? DateTime.Now,
                                                        Inn = inn,
                                                        management_reason = managementReason
                                                    });
                                        }

                                        // если он привязан уже к кому то, но не к текущей, а текущий период активный
                                        else if (period.State == ReportingPeriodStateEnum.current && inn != currentManagingInn)
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
                                                this.Performer.AddToQueue<SetUnlinkFromOrganizationAction, SetUnlinkFromOrganizationParams, object>()
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
                                                                this.Performer
                                                                    .AddToQueue<SetHouseLinkToOrganizationAction, SetHouseLinkToOrganizationParams, object>
                                                                    ()
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
                                                                        service.Save(
                                                                            new ChangedRobject
                                                                            {
                                                                                RealityObject = new RealityObject { Id = management.RobjectId }
                                                                            }));
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
                        var unlinkableRobjectIds = refRobjectIds.Where(x => data.ContainsKey(x)).ToArray();

                        // если есть
                        if (unlinkableRobjectIds.Length > 0)
                        {
                            // ищем последний контракт
                            var management = robjectManagement.Value.OrderByDescending(x => x.DateEnd).First();

                            // проходим по каждому дому реформы
                            foreach (var unlinkableRobjectId in unlinkableRobjectIds)
                            {
                                // и отвязываем от УО
                                this.Performer.AddToQueue<SetUnlinkFromOrganizationAction, SetUnlinkFromOrganizationParams, object>()
                                    .WithParameters(
                                        new SetUnlinkFromOrganizationParams
                                        {
                                            DateEnd = management.DateEnd ?? DateTime.Now,
                                            ExternalId = unlinkableRobjectId,
                                            Reason = management.TerminateReason,
                                            ReasonType =
                                                management.Return(x => x.ContractStopReason)
                                                    .Return(x => x == 0 ? ContractStopReasonEnum.finished_contract : x)
                                        })
                                    .WithCallback(
                                        actionResult =>
                                        {
                                            // если ошибка о незаполненной дате начала
                                            if (!actionResult.Success && actionResult.ErrorDetails.Code == ErrorCodes.MissingManagementDate)
                                            {
                                                // ставим дом в список домов для обновления профиля
                                                this.Container.UsingForResolved<IDomainService<ChangedRobject>>(
                                                    (container, service) =>
                                                        service.Save(
                                                            new ChangedRobject { RealityObject = new RealityObject { Id = management.RobjectId } }));
                                            }
                                        });
                            }
                        }

                        // Пытаемся завершить обслуживание оставшихся домов
                        this.Performer.AddToQueue<GetHouseInfoAction, long, HouseInfo[]>()
                            .WithParameters(robjectManagement.Key)
                            .WithCallback(new PerformerCallback<HouseInfo[]>(
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
                                                    var oldManagement =
                                                        history.OrderBy(x => x.DateEnd).LastOrDefault(x => x.ManOrgInn == currentManagingInn);

                                                    // отвязываем от текущей УО
                                                    this.Performer.AddToQueue<SetUnlinkFromOrganizationAction, SetUnlinkFromOrganizationParams, object>()
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
                                                            new PerformerCallback<object>(
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
                                                                                    service.Save(
                                                                                        new ChangedRobject
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
                this.Container.Release(reportingPeriodDomain);
            }
        }

        /// <summary>
        /// Извлечь параметры задачи
        /// </summary>
        /// <param name="params">Словарь параметров</param>
        /// <returns>Параметры</returns>
        protected override SetCompanyProfileParams ExtractParamsFromArgs(DynamicDictionary @params)
        {
            return new SetCompanyProfileParams
            {
                ManagingOrganizationId = @params.GetAsId("ManagingOrganizationId"),
                PeriodExternalId = @params.GetAs("PeriodExternalId", 0),
                PeriodId = @params.GetAsId("PeriodId")
            };
        }
    }
}