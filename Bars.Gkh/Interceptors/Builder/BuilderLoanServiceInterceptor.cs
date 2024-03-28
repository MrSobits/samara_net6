namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BuilderLoanServiceInterceptor : EmptyDomainInterceptor<BuilderLoan>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<BuilderLoan> service, BuilderLoan entity)
        {
            var builderLoanRepaymService = Container.Resolve<IDomainService<BuilderLoanRepayment>>();
            var builderLoanRepaymIds =
                builderLoanRepaymService.GetAll().Where(x => x.BuilderLoan.Id == entity.Id).Select(x => x.Id).ToArray();
            foreach (var id in builderLoanRepaymIds)
            {
                builderLoanRepaymService.Delete(id);
            }

            Container.Release(builderLoanRepaymService);

            return Success();
        }
    }
}
