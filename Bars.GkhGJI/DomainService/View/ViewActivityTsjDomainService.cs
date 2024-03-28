namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Entities;

    public class ViewActivityTsjDomainService : BaseDomainService<ViewActivityTsj>
    {
        public override IQueryable<ViewActivityTsj> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();

            return base.GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains((int) x.MunicipalityId))
                .WhereIf(contragentIds.Count > 0, x => contragentIds.Contains((int) x.ContragentId));
        }
    }
}