namespace Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;

    using Castle.Windsor;

    public class ConfirmContributionService : IConfirmContributionService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ManagOrgsList(BaseParams baseParams)
        {
            var managOrgDomain = Container.Resolve<IDomainService<ManagingOrganization>>();
            var confContribDomain = Container.Resolve<IDomainService<ConfirmContribution>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var managOrgsIds =
                    confContribDomain.GetAll()
                        .Where(x => x.ManagingOrganization != null)
                        .Select(x => x.ManagingOrganization.Id)
                        .ToArray();

                var data = managOrgDomain.GetAll()
                    .Where(x => x.OrgStateRole == OrgStateRole.Active)
                    .Where(x => !managOrgsIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        ManagingOrganizationName = x.Contragent.Name
                    })
                    .Filter(loadParams, Container);
                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                Container.Release(managOrgDomain);
                Container.Release(confContribDomain);
            }
        }

        public IDataResult RealObjList(BaseParams baseParams)
        {
            var realObjDomain = Container.Resolve<IDomainService<RealityObject>>();
            var confContribDomain = Container.Resolve<IDomainService<ConfirmContribution>>();
            var manOrgRealObj = Container.Resolve<IDomainService<ManagingOrgRealityObject>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var confContribId = baseParams.Params.GetAs<long>("confContribId");

                var confContrib = confContribDomain.Get(confContribId);
                
                var managOrg = confContrib.ManagingOrganization;

                var realObjIds = manOrgRealObj.GetAll()
                    .Where(x => x.ManagingOrganization.Id == managOrg.Id)
                    .Select(x => x.RealityObject.Id)
                    .ToArray();

                var data = realObjDomain.GetAll()
                    .Where(x => realObjIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        x.Address
                    })
                    .Filter(loadParams, Container);
                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                Container.Release(realObjDomain);
                Container.Release(confContribDomain);
                Container.Release(manOrgRealObj);
            }
        }
    }
}