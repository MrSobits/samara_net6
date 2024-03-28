namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BuilderLoanViewModel : BaseViewModel<BuilderLoan>
    {
        public override IDataResult List(IDomainService<BuilderLoan> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var builderId = baseParams.Params.GetAs<long>("builderId");

            var data = domain.GetAll()
                .Where(x => x.Builder.Id == builderId)
                .Select(x => new
                {
                    x.Id,
                    Lender = x.Lender.Name,
                    x.Amount,
                    x.DateIssue,
                    x.DatePlanReturn,
                    x.Description,
                    x.DocumentDate,
                    x.DocumentName,
                    x.DocumentNum
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}