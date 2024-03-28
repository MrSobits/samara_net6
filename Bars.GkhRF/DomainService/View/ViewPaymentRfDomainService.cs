namespace Bars.GkhRf.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Gkh.Entities;
    using Entities;

    public class ViewPaymentRfDomainService : BaseDomainService<ViewPayment>
    {
        public override IQueryable<ViewPayment> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();

            var serviceManOrgContractRobject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            return base.GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => x.MunicipalityId.HasValue && municipalityIds.Contains(x.MunicipalityId.Value))
                    .WhereIf(contragentIds.Count > 0, y => serviceManOrgContractRobject.GetAll()
                    .Any(x => x.RealityObject.Id == y.RealityObjectId
                        && contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id)
                        && x.ManOrgContract.StartDate <= DateTime.Now.Date
                        && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now.Date)));
        }
    }
}