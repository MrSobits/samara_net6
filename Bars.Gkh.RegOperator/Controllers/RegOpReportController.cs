namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4.IoC;
    using Decisions.Nso.Entities;
    using Gkh.Entities;
    using Gkh.Enums.Decisions;

    public class RegOpReportController : BaseController
    {
        public ActionResult RepairContributionInfoRobjectList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var domain = Container.ResolveDomain<RealityObject>();
            var crFundDecDomain = Container.ResolveDomain<CrFundFormationDecision>();
            var accOwnerDecDomain = Container.ResolveDomain<AccountOwnerDecision>();

            var reportDate = baseParams.Params.GetAs<DateTime?>("reportDate");
            var crFormFund = baseParams.Params.GetAs<CrFundFormationDecisionType?>("crFormFund");
            var accOwner = baseParams.Params.GetAs<AccountOwnerDecisionType?>("accOwner");

            using (Container.Using(domain, crFundDecDomain, accOwnerDecDomain))
            {
                var filterFormFund = crFundDecDomain.GetAll()
                    .WhereIf(crFormFund.HasValue && crFormFund.Value != CrFundFormationDecisionType.Unknown,
                        x => x.Decision == crFormFund.Value)
                    .WhereIf(reportDate.HasValue, x => x.Protocol.ProtocolDate <= reportDate.Value)
                    .Where(x => x.Protocol.State.FinalState);

                var filterOwner = accOwnerDecDomain.GetAll()
                    .WhereIf(accOwner.HasValue, x => x.DecisionType == accOwner.Value)
                    .WhereIf(reportDate.HasValue, x => x.Protocol.ProtocolDate <= reportDate.Value)
                    .Where(x => x.Protocol.State.FinalState);

                var data = domain.GetAll()
                    .Where(y => filterFormFund.Any(x => x.Protocol.RealityObject.Id == y.Id))
                    .Where(y => filterOwner.Any(x => x.Protocol.RealityObject.Id == y.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Address,
                        Municipality = x.Municipality.Name
                    })
                    .Filter(loadParams, Container);

                return new JsonListResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}
