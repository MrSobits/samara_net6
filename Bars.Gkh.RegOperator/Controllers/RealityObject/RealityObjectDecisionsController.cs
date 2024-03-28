using Bars.Gkh.Decisions.Nso.Domain;
using Bars.Gkh.Decisions.Nso.Entities;
using Bars.Gkh.DomainService;
using Bars.Gkh.Enums;
using Bars.Gkh.Enums.Decisions;

namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Entities;
    using Gkh.Domain;
    using Gkh.Entities;

    public class RealityObjectDecisionsController : BaseController
    {
        private readonly IDomainService<RealityObject> _roDomain;
        private readonly IRealityObjectDecisionsService _decisionService;
        private readonly IDomainService<DecisionNotification> _decisionNotificDomain;
        private readonly IDomainService<CrFundFormationDecision> _crFundDecDomain;
        private readonly IDomainService<MinFundAmountDecision> _minFundDecDomain;
        private readonly IDomainService<CreditOrgDecision> _creditOrgDecDomain;
        private readonly IDomainService<AccountOwnerDecision> _accOwnerDecDomain;
        private readonly IManagingOrgRealityObjectService _manOrgService;
        private readonly IRealityObjectDecisionProtocolService _decisionProtocolService;

        public RealityObjectDecisionsController(IDomainService<RealityObject> roDomain,
            IRealityObjectDecisionsService decisionService,
            IDomainService<CrFundFormationDecision> crFundDecDomain,
            IDomainService<AccountOwnerDecision> accOwnerDecDomain,
            IManagingOrgRealityObjectService manOrgService, 
            IDomainService<MinFundAmountDecision> minFundDecDomain,
            IDomainService<CreditOrgDecision> creditOrgDecDomain,
            IDomainService<DecisionNotification> decisionNotificDomain, 
            IRealityObjectDecisionProtocolService decisionProtocolService)
        {
            _roDomain = roDomain;
            _decisionService = decisionService;
            _crFundDecDomain = crFundDecDomain;
            _accOwnerDecDomain = accOwnerDecDomain;
            _manOrgService = manOrgService;
            _minFundDecDomain = minFundDecDomain;
            _creditOrgDecDomain = creditOrgDecDomain;
            _decisionNotificDomain = decisionNotificDomain;
            _decisionProtocolService = decisionProtocolService;
        }

        public ActionResult GetExistingSolutions(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("Id");

            var ro = _roDomain.Get(roId);

            if (ro == null)
            {
                return JsSuccess();
            }

            var currentProtocol = _decisionProtocolService.GetActiveProtocol(ro);

            var cd = _decisionService.GetActualDecision<CrFundFormationDecision>(ro, true);
            var ad = _decisionService.GetActualDecision<AccountOwnerDecision>(ro, true);
            var mf = _decisionService.GetActualDecision<MinFundAmountDecision>(ro, true);
            var creditOrg = _decisionService.GetActualDecision<CreditOrgDecision>(ro, true);

            var notification = currentProtocol != null ? _decisionNotificDomain.FirstOrDefault(x => x.Protocol.Id == currentProtocol.Id) : null;
            var manOrgC = _manOrgService.GetCurrentManOrg(ro);

            var regOpName = Container.Resolve<IDomainService<RegOperator>>().GetAll()
                    .Where(x => x.Contragent.ContragentState == ContragentState.Active)
                    .Select(x => x.Contragent.Name)
                    .FirstOrDefault();

            return JsSuccess(new
            {
                ManageStart = manOrgC.Return(x => x.StartDate),
                ManageEnd = manOrgC.Return(x => x.EndDate),
                ManageDecision =
                    manOrgC != null ? manOrgC.TypeContractManOrgRealObj.GetEnumMeta().Display : string.Empty,
                ManageUo = manOrgC.Return(x => x.ManagingOrganization).Return(x => x.Contragent).Return(x => x.Name),
                CrFundStart = cd.Return(x => x.Protocol).Return(x => x.ProtocolDate),
                CrFundEnd = string.Empty,
                CrFundDecision = cd != null ? cd.Decision.GetEnumMeta().Display : string.Empty,
                OwnerStart = ad.Return(x => x.Protocol).Return(x => x.ProtocolDate),
                OwnerEnd = string.Empty,
                OwnerDecision = ad != null ? ad.DecisionType.GetEnumMeta().Display : string.Empty,
                OwnerContragentType = ad.Return(x => x.DecisionType) == AccountOwnerDecisionType.Custom
                    ? (manOrgC != null && manOrgC.ManagingOrganization != null)
                        ? manOrgC.ManagingOrganization.TypeManagement.GetEnumMeta().Display
                        : string.Empty
                    : string.Empty,
                OwnerContragentName = ad.Return(x => x.DecisionType) == AccountOwnerDecisionType.Custom
                    ? manOrgC.Return(x => x.ManagingOrganization).Return(x => x.Contragent).Return(x => x.Name)
                    : regOpName,
                MinFundAmount = mf != null ? mf.Decision : 40, // ААА!!! костыль
                CreditOrg = creditOrg != null && creditOrg.Decision != null ? creditOrg.Decision.Name : string.Empty,
                AuthorizedPerson = currentProtocol.Return(x => x.AuthorizedPerson),
                AccountNumber = notification.Return(x => x.AccountNum)                  
            });
        }
    }
}