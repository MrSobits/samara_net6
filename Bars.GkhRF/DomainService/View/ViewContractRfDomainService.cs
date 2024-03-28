namespace Bars.GkhRf.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhRf.Entities;

    public class ViewContractRfDomainService : BaseDomainService<ViewContractRf>
    {
        public override IQueryable<ViewContractRf> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();

            return
                base.GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => x.MunicipalityId.HasValue && municipalityIds.Contains(x.MunicipalityId.Value))
                    .WhereIf(contragentIds.Count > 0, x => x.ContragentId.HasValue && contragentIds.Contains(x.ContragentId.Value));
        }    }
}