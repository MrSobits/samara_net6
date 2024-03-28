namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Castle.Windsor;
    using Domain;

    public class ManagingOrganizationService : IManagingOrganizationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetManOrgByContagentId(BaseParams baseParams)
        {
            var contragentId = baseParams.Params.GetAsId("contragentId");

            var manOrgDomain = Container.ResolveDomain<ManagingOrganization>();

            try
            {
                var manOrg = manOrgDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == contragentId);

                return new BaseDataResult(manOrg);
            }
            finally
            {
                Container.Release(manOrgDomain);
            }
        }
    }
}