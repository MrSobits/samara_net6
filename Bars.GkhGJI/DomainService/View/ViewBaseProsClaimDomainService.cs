namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Entities;

    public class ViewBaseProsClaimDomainService : BaseDomainService<ViewBaseProsClaim>
    {
        //public override IQueryable<ViewBaseProsClaim> GetAll()
        //{
        //    var userManager = Container.Resolve<IGkhUserManager>();

        //    var inspectorIds = userManager.GetInspectorIds();
        //    var municipalityIds = userManager.GetMunicipalityIds();

        //    var serviceInspectionInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();

        //    return base.GetAll()
        //        .WhereIf(municipalityIds.Count > 0, x => x.MunicipalityId.HasValue && municipalityIds.Contains(x.MunicipalityId.Value))
        //        .WhereIf(inspectorIds.Count > 0, y => serviceInspectionInspector.GetAll()
        //            .Any(x => x.Inspection.Id == y.Id && inspectorIds.Contains(x.Inspector.Id)));
        //}
    }
}