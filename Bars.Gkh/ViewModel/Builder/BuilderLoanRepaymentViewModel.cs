namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BuilderLoanRepaymentViewModel : BaseViewModel<BuilderLoanRepayment>
    {
        public override IDataResult List(IDomainService<BuilderLoanRepayment> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var builderLoanId = baseParams.Params.GetAs<long>("builderLoanId");

            var data = domain.GetAll()
                .Where(x => x.BuilderLoan.Id == builderLoanId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.RepaymentAmount,
                    x.RepaymentDate
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}