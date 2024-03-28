namespace Bars.Gkh1468.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    public class PublicServiceOrgDomainService : BaseDomainService<PublicServiceOrg>
    {
        public override IQueryable<PublicServiceOrg> GetAll()
        {
            var municipalityIds = Container.Resolve<IGkhUserManager>().GetMunicipalityIds();

            return base.GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Contragent.Municipality.Id));
        }
    }
}