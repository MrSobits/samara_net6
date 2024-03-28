namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Entities;

    public class ViewBaseDefaultDomainService : BaseDomainService<ViewBaseDefault>
    {
        public override IQueryable<ViewBaseDefault> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityList = userManager.GetMunicipalityIds();

            return base.GetAll()
                    .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value));
        }
    }
}