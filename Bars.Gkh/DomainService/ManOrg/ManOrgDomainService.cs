namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using B4.Modules.FileStorage.DomainService;
    using Bars.Gkh.Entities;

    public class ManOrgDomainService : FileStorageDomainService<ManagingOrganization>
    {
        public IDomainService<ContragentMunicipality> ContragentMunicipalityDomain { get; set; } 

        public override IQueryable<ManagingOrganization> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var contragentList = userManager.GetContragentIds();
            var municipalityList = userManager.GetMunicipalityIds();

            return base.GetAll()
                .WhereIf(contragentList.Count > 0, x => contragentList.Contains(x.Contragent.Id))
                .WhereIf(municipalityList.Count > 0,
                    x =>
                        municipalityList.Contains(x.Contragent.Municipality.Id) || ContragentMunicipalityDomain.GetAll()
                            .Where(m => m.Contragent.Id == x.Contragent.Id)
                            .Any(m => municipalityList.Contains(m.Municipality.Id)));
        }
    }
}