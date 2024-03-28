namespace Bars.Gkh.Reforma.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.GkhDi.Entities;

    public class PeriodDiInterceptor : EmptyDomainInterceptor<PeriodDi>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<PeriodDi> service, PeriodDi entity)
        {
            var periodService = this.Container.ResolveDomain<ReportingPeriodDict>();
            try
            {
                periodService.GetAll().Where(x => x.PeriodDi.Id == entity.Id).ForEach(
                    x =>
                        {
                            x.PeriodDi = null;
                            periodService.Update(x);
                        });

                return this.Success();
            }
            finally
            {
                this.Container.Release(periodService);
            }
        }
    }
}