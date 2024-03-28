using Bars.B4;
using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;
using System.Linq;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.ViewModel
{
    class AgentPIRDebtorCreditViewModel : BaseViewModel<AgentPIRDebtorCredit>
    {
        public override IDataResult List(IDomainService<AgentPIRDebtorCredit> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var agentPIRDebId = loadParams.Filter.GetAs("agentPIRDebtorId", 0L);

            var data = domain.GetAll()
                .Where(x => x.Debtor.Id == agentPIRDebId)
                .Select(x => new
                {
                    x.Id,
                    x.Debtor,
                    x.Credit,
                    x.Date,
                    x.User,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

    }
}
