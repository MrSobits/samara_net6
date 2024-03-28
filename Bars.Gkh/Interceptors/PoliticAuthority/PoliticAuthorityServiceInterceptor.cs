namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class PoliticAuthorityServiceInterceptor : EmptyDomainInterceptor<PoliticAuthority>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<PoliticAuthority> service, PoliticAuthority entity)
        {
            return service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id)
                ? Failure("Орган государственной власти с таким контрагентом уже создан")
                : Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PoliticAuthority> service, PoliticAuthority entity)
        {
            var politicAuthMunicipalityService = Container.Resolve<IDomainService<PoliticAuthorityMunicipality>>();
            var politicAuthMunicipalityList = politicAuthMunicipalityService.GetAll().Where(x => x.PoliticAuthority.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in politicAuthMunicipalityList)
            {
                politicAuthMunicipalityService.Delete(value);
            }

            var politicAuthWorkModeService = Container.Resolve<IDomainService<PoliticAuthorityWorkMode>>();
            var politicAuthWorkModeList = politicAuthWorkModeService.GetAll().Where(x => x.PoliticAuthority.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in politicAuthWorkModeList)
            {
                politicAuthWorkModeService.Delete(value);
            }

            return this.Success();
        }
    }
}
