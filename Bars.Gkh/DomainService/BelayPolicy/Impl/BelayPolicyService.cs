namespace Bars.Gkh.DomainService.Impl
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class BelayPolicyService : IBelayPolicyService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            var belayPolicy = Container.Resolve<IDomainService<BelayPolicy>>().Get(baseParams.Params.GetAs<long>("belayPolicyId"));
            return new BaseDataResult(new {manOrgId = belayPolicy.BelayManOrgActivity.ManagingOrganization.Id});
        }
    }
}