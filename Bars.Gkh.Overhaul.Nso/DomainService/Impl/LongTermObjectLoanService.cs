namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.Linq;
    using B4;
    using Castle.Windsor;
    using Entities;

    public class LongTermObjectLoanService : ILongTermObjectLoanService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListRegister(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = Container.Resolve<IDomainService<LongTermObjectLoan>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    ObjectAddress = x.Object.RealityObject.Address,
                    x.Object,
                    ObjectIssued = x.ObjectIssued.RealityObject.Address,
                    x.PeriodLoan,
                    x.LoanAmount,
                    x.DateIssue,
                    x.DateRepayment
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
