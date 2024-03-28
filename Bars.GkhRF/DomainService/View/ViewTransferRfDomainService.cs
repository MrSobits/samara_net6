namespace Bars.GkhRf.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Entities;

    public class ViewTransferRfDomainService : BaseDomainService<ViewTransferRf>
    {
        public override IQueryable<ViewTransferRf> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();

            return base.GetAll()
                    .WhereIf(contragentIds.Count > 0, x => x.ContragentId.HasValue && contragentIds.Contains(x.ContragentId.Value))
                    .WhereIf(municipalityIds.Count > 0, x => x.MunicipalityId.HasValue && municipalityIds.Contains(x.MunicipalityId.Value));
        }
    }
}