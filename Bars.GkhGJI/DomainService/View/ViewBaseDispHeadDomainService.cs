namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Entities;

    public class ViewBaseDispHeadDomainService : BaseDomainService<ViewBaseDispHead>
    {
        public override IQueryable<ViewBaseDispHead> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var inspectorList = userManager.GetInspectorIds();
            var municipalityList = userManager.GetMunicipalityIds();

            var serviceInspectionInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();

            return base.GetAll()
                .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value));
                //.WhereIf(inspectorList.Count > 0, y => serviceInspectionInspector.GetAll()
                //    .Any(x => y.Id == x.Inspection.Id
                //        && (inspectorList.Contains(x.Inspector.Id) || (y.HeadId.HasValue && inspectorList.Contains(y.HeadId.Value)))));
        }
    }
}