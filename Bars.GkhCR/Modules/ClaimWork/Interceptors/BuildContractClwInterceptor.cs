namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.Interceptors;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Modules.ClaimWork.Entities;

    public class BuildContractClwInterceptor : BaseClaimWorkInterceptor<BuildContractClaimWork>
    {
        public override IDataResult BeforeDeleteAction(
            IDomainService<BuildContractClaimWork> service,
            BuildContractClaimWork entity)
        {
            var violDomain = Container.Resolve<IDomainService<BuildContractClwViol>>();
            try
            {
                var violList = violDomain.GetAll().Where(x => x.ClaimWork.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var viol in violList)
                {
                    violDomain.Delete(viol);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(violDomain);
            }
        }
    }
}