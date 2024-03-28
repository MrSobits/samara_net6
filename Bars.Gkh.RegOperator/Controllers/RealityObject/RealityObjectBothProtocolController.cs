namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;

    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Decisions.Nso.Entities;
    using Decisions.Nso.Entities.Decisions;
    using Enums;
    using Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectProtocol;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Контроллер протоколов
    /// </summary>
    public class RealityObjectBothProtocolController : BaseController
    {
        /// <summary>
        /// Интерфейс для сохранения или изменения решений?
        /// </summary>
        public IRealityObjectBothProtocolService Service { get; set; }

        /// <summary>
        /// ManOrgService
        /// </summary>
        public IManagingOrgRealityObjectService ManOrgService { get; set; }

        /// <summary>Получить список протоколов</summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var roId = baseParams.Params.GetAsId("roId");

            var protocols = new List<ProtocolProxy>();

            var availableTypes = this.Service.GetAvaliableTypes(baseParams);

            if (availableTypes.Get(CoreDecisionType.Owners))
            {
                protocols.AddRange(this.Container.ResolveDomain<RealityObjectDecisionProtocol>().GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Select(x => new ProtocolProxy
                    {
                        Id = x.Id,
                        DocumentNum = x.DocumentNum,
                        ProtocolDate = x.ProtocolDate,
                        ProtocolType = CoreDecisionType.Owners,
                        State = x.State,
                        CrFundFormationType = this.GetCrFundFormationType(x.Id),
                        AccountOwnerDecisionType = this.GetAccountOwnerDecisionType(x.Id),
                        ManOrgName = this.GetManOrgName(roId),
                        DateStart = x.DateStart,
                        LetterNumber = x.LetterNumber,
                        LetterDate = x.LetterDate
                    })
                    .AsEnumerable());
            }

            if (availableTypes.Get(CoreDecisionType.Government))
            {
                protocols.AddRange(this.Container.ResolveDomain<GovDecision>().GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Select(x => new ProtocolProxy
                    {
                        Id = x.Id,
                        DocumentNum = x.ProtocolNumber,
                        ProtocolDate = x.ProtocolDate,
                        ProtocolType = CoreDecisionType.Government,
                        State = x.State,
                        CrFundFormationType = x.FundFormationByRegop ? CrFundFormationType.RegOpAccount : CrFundFormationType.NotSelected,
                        ManOrgName = this.GetManOrgName(roId),
                        DateStart = x.DateStart,
                        LetterNumber = x.LetterNumber,
                        LetterDate = x.LetterDate
                    })
                    .AsEnumerable());
            }

            var baseDecisionProtocols = this.Container.ResolveDomain<BaseDecisionProtocol>().GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .AsEnumerable()
                .Select(
                    x =>
                    {
                        var protocolType = CoreDecisionType.CrFund;
                        if (x is CrFundDecisionProtocol)
                        {
                            protocolType = CoreDecisionType.CrFund;
                        }
                        else if (x is ManagementOrganizationDecisionProtocol)
                        {
                            protocolType = CoreDecisionType.ManagementOrganization;
                        }
                        else if (x is MkdManagementTypeDecisionProtocol)
                        {
                            protocolType = CoreDecisionType.MkdManagementType;
                        }
                        else if (x is OoiManagementDecisionProtocol)
                        {
                            protocolType = CoreDecisionType.OoiManagement;
                        }
                        else if (x is TariffApprovalDecisionProtocol)
                        {
                            protocolType = CoreDecisionType.TariffApproval;
                        }
                        return new ProtocolProxy
                        {
                            Id = x.Id,
                            DocumentNum = x.ProtocolNumber,
                            ProtocolDate = x.ProtocolDate,
                            ProtocolType = protocolType,
                            State = x.State
                        };
                    }
                )
                .Where(x => availableTypes.Get(x.ProtocolType))
                .ToList();

            protocols.AddRange(baseDecisionProtocols);

            return new JsonListResult(protocols.AsQueryable().Filter(loadParams, this.Container).Order(loadParams).Paging(loadParams).ToList(), protocols.Count);
        }

        /// <summary>
        /// Получить протокол решений
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult Get(BaseParams baseParams)
        {
            var result = this.Service.Get(baseParams);
            return new JsonNetResult(result.Data);
        }

        /// <summary>
        /// Сохранить протоколы решений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult SaveOrUpdateDecisions(BaseParams baseParams)
        {
            // стремная магия, решающая проблемы кроссбраузерности — IE и Firefox
            // не хотят парсить ответ на POST-запрос, пришедший с другим Content-Type
            // аналогично \Bars.Gkh\Controllers\ParametersController.cs
            var response = new JsonNetResult(this.Service.SaveOrUpdateDecisions(baseParams))
            {
                ContentType = "text/html"
            };

            return response;
        }

        /// <summary>
        /// Удалить протокол решения
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult Delete(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.Delete(baseParams));
        }

        /// <summary>
        /// Получить доступные типы протоколов
        /// </summary>
        public ActionResult GetAvaliableTypes(BaseParams baseParams)
        {
            var result = this.Service.GetAvaliableTypes(baseParams);
            return new JsonNetResult(result.Where(x => x.Value).Select(x => (int)x.Key));
        }

        /// <summary>
        /// Получить способ формирования фонда КР для протокола решения собственников
        /// </summary>
        /// <param name="protocolId"></param>
        /// <returns></returns>
        private CrFundFormationType? GetCrFundFormationType(long protocolId)
        {
            var crFundFormationType = CrFundFormationType.Unknown;

            var crFundFormationDecisionDomain = this.Container.ResolveDomain<CrFundFormationDecision>();
            var accountOwnerDecisionDomain = this.Container.ResolveDomain<AccountOwnerDecision>();
            using (this.Container.Using(crFundFormationDecisionDomain, accountOwnerDecisionDomain))
            {
                var crFundFormationDecision = crFundFormationDecisionDomain.GetAll().Where(x => x.Protocol.Id == protocolId && x.IsChecked).FirstOrDefault();

                if (crFundFormationDecision != null)
                {
                    var crFundFormationDecisionType = crFundFormationDecision.Decision;

                    var accountOwnerDecision = accountOwnerDecisionDomain.GetAll().Where(x => x.Protocol.Id == protocolId && x.IsChecked).FirstOrDefault();

                    if (accountOwnerDecision != null)
                    {
                        AccountOwnerDecisionType accountOwnerDecisionType = accountOwnerDecision.DecisionType;

                        // Спец. счет на дом
                        if (crFundFormationDecisionType == CrFundFormationDecisionType.SpecialAccount
                            && accountOwnerDecisionType == AccountOwnerDecisionType.Custom)
                        {
                            crFundFormationType = CrFundFormationType.SpecialAccount;
                        }

                        // Спец счет регопа
                        else if (crFundFormationDecisionType == CrFundFormationDecisionType.SpecialAccount
                            && accountOwnerDecisionType == AccountOwnerDecisionType.RegOp)
                        {
                            crFundFormationType = CrFundFormationType.SpecialRegOpAccount;
                        }
                    }
                    // Счет регопа
                    else if (crFundFormationDecisionType == CrFundFormationDecisionType.RegOpAccount)
                    {
                        crFundFormationType = CrFundFormationType.RegOpAccount;
                    }
                }
            }

            return crFundFormationType;
        }

        /// <summary>
        /// Получить владельца счета
        /// </summary>
        /// <param name="protocolId"></param>
        /// <returns></returns>
        private AccountOwnerDecisionType? GetAccountOwnerDecisionType(long protocolId)
        {
            var crFundFormationDecisionDomain = this.Container.ResolveDomain<CrFundFormationDecision>();
            var accountOwnerDecisionDomain = this.Container.ResolveDomain<AccountOwnerDecision>();

            AccountOwnerDecisionType? accountOwnerDecisionType = null;

            using (this.Container.Using(crFundFormationDecisionDomain, accountOwnerDecisionDomain))
            {
                var crFundFormationDecisionType = crFundFormationDecisionDomain.GetAll()
                    .WhereNotNull(x => x.Protocol)
                    .FirstOrDefault(x => x.Protocol.Id == protocolId && x.IsChecked)
                    .Return(x => x.Decision, (CrFundFormationDecisionType?) null);

                if (crFundFormationDecisionType == CrFundFormationDecisionType.SpecialAccount)
                {
                    accountOwnerDecisionType = accountOwnerDecisionDomain.GetAll()
                        .WhereNotNull(x => x.Protocol)
                        .FirstOrDefault(x => x.Protocol.Id == protocolId && x.IsChecked)
                        .Return(x => x.DecisionType, (AccountOwnerDecisionType?) null);
                }

                return accountOwnerDecisionType;
            }
        }

        /// <summary>
        /// Получить управление домом
        /// </summary>
        /// <param name="roId"></param>
        /// <returns></returns>
        private string GetManOrgName(long roId)
        {
            string manOrgName = string.Empty;

            var manOrgContract = this.ManOrgService.GetCurrentManOrg(new RealityObject { Id = roId });

            if (manOrgContract != null)
            {
                if (manOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                {
                    manOrgName = TypeContractManOrg.DirectManag.GetEnumMeta().Display;
                }
                else if (manOrgContract.ManagingOrganization != null)
                {
                    manOrgName = manOrgContract.ManagingOrganization.Contragent.Name;
                }
            }
            return manOrgName;
        }

        private class ProtocolProxy
        {
            public long Id { get; set; }
            public string DocumentNum { get; set; }
            public DateTime ProtocolDate { get; set; }
            public CoreDecisionType ProtocolType { get; set; }
            public State State { get; set; }
            public CrFundFormationType? CrFundFormationType { get; set; }
            public AccountOwnerDecisionType? AccountOwnerDecisionType { get; set; }
            public string ManOrgName { get; set; }
            public DateTime? DateStart { get; set; }
            public string LetterNumber { get; set; }
            public DateTime? LetterDate { get; set; }
        }
    }
}