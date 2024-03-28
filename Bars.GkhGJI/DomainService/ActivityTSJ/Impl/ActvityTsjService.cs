namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using Castle.Windsor;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Authentification;

    public class ActivityTsjService : IActivityTsjService
    {
        public IWindsorContainer Container { get; set; }

        public IQueryable<ActivityTsj> GetFilteredByOperator(IDomainService<ActivityTsj> domainService)
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var contragentIds = userManager.GetContragentIds();
            var municipalityIds = userManager.GetMunicipalityIds();

            return domainService.GetAll()
                .WhereIf(contragentIds.Count > 0, x => contragentIds.Contains(x.ManagingOrganization.Contragent.Id))
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.ManagingOrganization.Contragent.Municipality.Id));
        }
    }
}