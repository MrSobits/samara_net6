namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;

    public class PoliticAuthorityDomainService : BaseDomainService<PoliticAuthority>
    {
        public override IQueryable<PoliticAuthority> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var contragentList = userManager.GetContragentIds();
            var municipalityList = userManager.GetMunicipalityIds();

            return base.GetAll()
                .WhereIf(contragentList.Count > 0, x => contragentList.Contains(x.Contragent.Id))
                .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.Contragent.Municipality.Id));
        }
    }
}