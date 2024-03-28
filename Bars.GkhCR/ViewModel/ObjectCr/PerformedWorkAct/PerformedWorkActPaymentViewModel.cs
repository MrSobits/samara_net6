namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;

    using Bars.Gkh.DataResult;

    using Entities;
    using NHibernate.Mapping;
    using Gkh.Domain;

    public class PerformedWorkActPaymentViewModel : BaseViewModel<PerformedWorkActPayment>
    {
        public override IDataResult List(IDomainService<PerformedWorkActPayment> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var performedWorkActId = loadParams.Filter.GetAsId("performedWorkActId");

            var data = domainService.GetAll()
                .Where(x => x.PerformedWorkAct.Id == performedWorkActId)
                .Filter(loadParams, Container);

            var summarySum = data.Sum(x => (decimal?)x.Sum);
            var summaryPercent = data.Sum(x => (decimal?)x.Percent);
            var summaryPaid = data.Sum(x => (decimal?)x.Paid);

            var totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            return new ListSummaryResult(data.ToList(), totalCount, new { Sum = summarySum, Percent = summaryPercent, Paid = summaryPaid });
        }
    }
}