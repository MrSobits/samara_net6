namespace Bars.Gkh.DomainService
{
    using System;
    using System.Linq;
    
    using B4;
    using B4.Utils;
    using Entities;
    using Authentification;

    using Castle.Windsor;

    public class EmergencyObjectService : IEmergencyObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IQueryable<EmergencyObject> GetFilteredByOperator()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();

            var serviceManOrgContractRobject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            return this.Container.Resolve<IDomainService<EmergencyObject>>().GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(contragentIds.Count > 0, y => serviceManOrgContractRobject.GetAll()
                    .Any(x => x.RealityObject.Id == y.RealityObject.Id
                        && contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id)
                        && x.ManOrgContract.StartDate <= DateTime.Now.Date
                        && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now.Date)));
        }
    }
}