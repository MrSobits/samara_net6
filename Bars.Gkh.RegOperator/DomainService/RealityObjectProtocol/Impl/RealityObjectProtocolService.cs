namespace Bars.Gkh.RegOperator.DomainService.RealityObjectProtocol.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.EntityHistory;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для сохранения или изменения решений
    /// </summary>
    public class RealityObjectBothProtocolService : IRealityObjectBothProtocolService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Интерфейс работы с менеджером логов
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Интерфейс описывающий сервис авторизации
        /// </summary>
        public IAuthorizationService AuthorizationService { get; set; }

        public IEntityHistoryService<GovDecision> GovDecisionHistoryService { get; set; }

        public IEntityHistoryService<RealityObjectDecisionProtocol> OwnerDecisionHistoryService { get; set; }

        /// <summary>
        /// Сохранить протоколы решений
        /// </summary>
        public IDataResult SaveOrUpdateDecisions(BaseParams baseParams)
        {
            var protocolType = baseParams.Params.GetAs<CoreDecisionType>("protocolT");
            try
            {
                switch (protocolType)
                {
                    case CoreDecisionType.Owners:
                        return this.SaveOwnersDecisions(baseParams);
                    case CoreDecisionType.Government:
                        return this.SaveGovermentDecisions(baseParams);
                    default:
                        return this.SaveProtocolDecisions(baseParams);
                }
            }
            catch (Exception e)
            {
                this.Logger.LogError(e.ToString());

                return new BaseDataResult(false, e.Message);
            }
        }
        
        private IDataResult SaveOwnersDecisions(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("roId");

            var specialCalcAccountService = this.Container.Resolve<ISpecialCalcAccountService>();
            var robjectDecisionService = this.Container.Resolve<IRobjectDecisionService>();
            using (this.Container.Using(specialCalcAccountService, robjectDecisionService))
            {
                return this.Container.InTransactionWithResult(() =>
                {
                    var result = robjectDecisionService.SaveOrUpdateDecisions(baseParams);
                    // актуализируем спецсчета по необходимости
                    specialCalcAccountService.ValidateActualSpecAccount(roId);

                    return result;
                });
            }
        }

        private IDataResult SaveGovermentDecisions(BaseParams baseParams)
        {
            var specialCalcAccountService = this.Container.Resolve<ISpecialCalcAccountService>();
            var robjectDecisionService = this.Container.Resolve<IRobjectDecisionService>();
            var govDecisionDomain = this.Container.ResolveDomain<GovDecision>();

            using (this.Container.Using(specialCalcAccountService, robjectDecisionService, govDecisionDomain))
            {
                return this.Container.InTransactionWithResult(() =>
                {
                    var hasNewFile = false;
                    var roId = baseParams.Params.GetAsId("roId");
                    var decision = this.GetSaveParam<GovDecision>(baseParams)?.FirstOrDefault()?.AsObject();


                    var protocolFile = baseParams.Files.Get("ProtocolFile");
                    var npaFile = baseParams.Files.Get("NpaFile");

                    if (protocolFile != null || npaFile != null)
                    {
                        var fileManager = this.Container.Resolve<IFileManager>();
                        using (this.Container.Using(fileManager))
                        {
                            if (protocolFile != null)
                            {
                                decision.ProtocolFile = fileManager.SaveFile(protocolFile);
                            }

                            if (npaFile != null)
                            {
                                decision.NpaFile = fileManager.SaveFile(npaFile);
                            }
                        }

                        hasNewFile = true;
                    }

                    var contract = this.Container.Resolve<IManagingOrgRealityObjectService>().GetManOrgOnDate(new RealityObject { Id = roId }, decision.DateStart);

                    if (contract != null)
                    {
                        decision.RealtyManagement = contract.ManagingOrganization?.Contragent.ShortName;
                    }

                    decision.RealityObject = decision.RealityObject ?? new RealityObject { Id = roId };

                    if (decision.Id > 0)
                    {
                        this.GovDecisionHistoryService.StoreEntity(decision.Id);

                        var protocolInfo = govDecisionDomain.GetAll()
                            .Where(x => x.Id == decision.Id)
                            .Select(x => new
                            {
                                x.ProtocolFile,
                                x.NpaFile,
                                x.State
                            })
                            .FirstOrDefault();

                        if (!hasNewFile)
                        {
                            decision.ProtocolFile = protocolInfo.Return(x => x.ProtocolFile);
                            decision.NpaFile = protocolInfo.Return(x => x.NpaFile);
                        }

                        decision.State = protocolInfo.Return(x => x.State);

                        govDecisionDomain.Update(decision);
                        this.GovDecisionHistoryService.LogUpdate(decision, decision.RealityObject);
                    }
                    else
                    {
                        govDecisionDomain.Save(decision);
                        this.GovDecisionHistoryService.LogCreate(decision, decision.RealityObject);
                    }
                    // актуализируем спецсчета по необходимости
                    specialCalcAccountService.ValidateActualSpecAccount(roId);

                    return new BaseDataResult(decision);
                });
            }
        }

        private IDataResult SaveProtocolDecisions(BaseParams baseParams)
        {
            var specialCalcAccountService = this.Container.Resolve<ISpecialCalcAccountService>();
            var robjectDecisionService = this.Container.Resolve<IRobjectDecisionService>();
            var crFundDomain = this.Container.ResolveDomain<BaseDecisionProtocol>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            using (this.Container.Using(specialCalcAccountService, robjectDecisionService, crFundDomain, stateProvider))
            {
                var roId = baseParams.Params.GetAsId("roId");
                var protocolType = baseParams.Params.GetAs<CoreDecisionType>("protocolT");

                BaseDecisionProtocol baseDecision;
                switch (protocolType)
                {
                    case CoreDecisionType.CrFund:
                        baseDecision = baseParams.Params.ReadClass<CrFundDecisionProtocol>();
                        break;
                    case CoreDecisionType.MkdManagementType:
                        baseDecision = baseParams.Params.ReadClass<MkdManagementTypeDecisionProtocol>();
                        break;
                    case CoreDecisionType.ManagementOrganization:
                        baseDecision = baseParams.Params.ReadClass<ManagementOrganizationDecisionProtocol>();
                        break;
                    case CoreDecisionType.TariffApproval:
                        baseDecision = baseParams.Params.ReadClass<TariffApprovalDecisionProtocol>();
                        break;
                    case CoreDecisionType.OoiManagement:
                        baseDecision = baseParams.Params.ReadClass<OoiManagementDecisionProtocol>();
                        break;
                    default:
                        baseDecision = baseParams.Params.ReadClass<BaseDecisionProtocol>();
                        break;
                }
                var hasNewFile = false;
                var file = baseParams.Files.FirstOrDefault().Return(x => x.Value);
                if (file != null)
                {
                    var fileManager = this.Container.Resolve<IFileManager>();
                    using (this.Container.Using(fileManager))
                    {
                        baseDecision.ProtocolFile = fileManager.SaveFile(file);
                    }

                    hasNewFile = true;
                }

                baseDecision.RealityObject = baseDecision.RealityObject ?? new RealityObject { Id = roId };
                if (baseDecision.Id > 0)
                {
                    var protocolInfo = crFundDomain.GetAll()
                        .Where(x => x.Id == baseDecision.Id)
                        .Select(x => new
                        {
                            Protocol = x.ProtocolFile,
                            x.State
                        })
                        .FirstOrDefault();
                    if (!hasNewFile)
                    {
                        baseDecision.ProtocolFile = protocolInfo.Return(x => x.Protocol);
                    }

                    baseDecision.State = protocolInfo.Return(x => x.State);
                }
                else
                {
                    stateProvider.SetDefaultState(baseDecision);
                }

                this.SaveOrUpdateBaseDecision(baseDecision);

                return new BaseDataResult(baseDecision);
            }
        }
        /// <summary>
        /// Получить протокол решений
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult Get(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("protocolId");
            var protocolType = baseParams.Params.GetAs<CoreDecisionType>("protocolT");

            if (protocolType == CoreDecisionType.Owners)
            {
                var robjectDecisionService = this.Container.Resolve<IRobjectDecisionService>();
                try
                {
                    var getResult = robjectDecisionService.Get(baseParams);
                    return new BaseDataResult(new {success = true, data = getResult.Data, protocolType = 10});
                }
                finally
                {
                    this.Container.Release(robjectDecisionService);
                }
            }
            if (protocolType == CoreDecisionType.Government)
            {
                var domain = this.Container.ResolveDomain<GovDecision>();
                var govDecisionService = this.Container.Resolve<IViewModel<GovDecision>>();
                baseParams.Params["id"] = id;
                try
                {
                    var govDecision = govDecisionService.Get(domain, baseParams);
                    return new BaseDataResult(new {success = true, data = govDecision.Data, protocolType = 20});
                }
                finally
                {
                    this.Container.Release(domain);
                    this.Container.Release(govDecisionService);
                }
            }

            var baseDecisionDomain = this.Container.ResolveDomain<BaseDecisionProtocol>();
            var baseDecisionViewModel = this.Container.Resolve<IViewModel<BaseDecisionProtocol>>();
            baseParams.Params["id"] = id;
            try
            {
                var baseDecision = baseDecisionViewModel.Get(baseDecisionDomain, baseParams);
                return new BaseDataResult(new {success = true, data = baseDecision.Data, protocolType });
            }
            finally
            {
                this.Container.Release(baseDecisionDomain);
                this.Container.Release(baseDecisionViewModel);
            }
        }

        /// <summary>
        /// Удалить протокол решения
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult Delete(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("id");
            var type = baseParams.Params.GetAs<CoreDecisionType>("protocolType");

            if (type == CoreDecisionType.Owners)
            {
                var robjectDecisionProtocolService = this.Container.ResolveDomain<RealityObjectDecisionProtocol>();
                try
                {
                    var roId = robjectDecisionProtocolService.GetAll()
                        .Where(x => x.Id == id)
                        .Select(x => x.RealityObject.Id)
                        .FirstOrDefault();

                    this.OwnerDecisionHistoryService.StoreEntity(id);
                    robjectDecisionProtocolService.Delete(id);
                    this.OwnerDecisionHistoryService.LogDelete(new RealityObject { Id = roId });
                }
                finally
                {
                    this.Container.Release(robjectDecisionProtocolService);
                }
            }
            else
            {
                if (type == CoreDecisionType.Government)
                {
                    var govDecisionService = this.Container.ResolveDomain<GovDecision>();
                    try
                    {
                        var roId = govDecisionService.GetAll()
                            .Where(x => x.Id == id)
                            .Select(x => x.RealityObject.Id)
                            .FirstOrDefault();

                        this.GovDecisionHistoryService.StoreEntity(id);
                        govDecisionService.Delete(id);
                        this.GovDecisionHistoryService.LogDelete(new RealityObject { Id = roId });
                    }
                    finally
                    {
                        this.Container.Release(govDecisionService);
                    }
                }
                else
                {
                    var baseDecisionService = this.Container.ResolveDomain<BaseDecisionProtocol>();
                    try
                    {
                        baseDecisionService.Delete(id);
                    }
                    finally
                    {
                        this.Container.Release(baseDecisionService);
                    }
                }
            }
        
            return new BaseDataResult();
        }
        
        /// <summary>
        /// Сохранить/обновить решение
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="protocol"></param>
        public void SaveOrUpdateBaseDecision<T>(T protocol) where T : BaseDecisionProtocol
        {
            var domain = this.Container.ResolveDomain<T>();
            try
            {
                if (protocol.Id > 0)
                {
                    domain.Update(protocol);
                }
                else
                {
                    domain.Save(protocol);
                }
            }
            finally
            {
                this.Container.Release(domain);
            }
        }

        /// <inheritdoc />
        public IDictionary<CoreDecisionType, bool> GetAvaliableTypes(BaseParams baseParams)
        {
            const string permissionPrefix = "Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.";

            var identity = this.Container.Resolve<IUserIdentity>();
            var result = new Dictionary<CoreDecisionType, bool>();

            foreach (CoreDecisionType coreDecisionType in Enum.GetValues(typeof(CoreDecisionType)))
            {
                result[coreDecisionType] = this.AuthorizationService.Grant(identity, permissionPrefix + coreDecisionType);
            }

            return result;
        }

        private SaveParam<T> GetSaveParam<T>(BaseParams baseParams)
            where T : PersistentObject
        {
            return baseParams.Params.Read<SaveParam<T>>().Execute(container => B4.DomainService.BaseParams.Converter.ToSaveParam<T>(container, true));
        }
    }
}