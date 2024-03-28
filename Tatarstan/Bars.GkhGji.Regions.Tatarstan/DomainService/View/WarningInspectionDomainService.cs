namespace Bars.GkhGji.Regions.Tatarstan.DomainService.View
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;

    public class WarningInspectionDomainService : BaseDomainService<ViewWarningInspection>
    {
        public override IQueryable<ViewWarningInspection> GetAll()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var inspectorList = userManager.GetInspectorIds();
            var serviceInspectionInspector = this.Container.Resolve<IDomainService<InspectionGjiInspector>>();

            return base.GetAll()
                .WhereIf(inspectorList.Count > 0, y => serviceInspectionInspector.GetAll()
                    .Any(x => y.Id == x.Inspection.Id && inspectorList.Contains(x.Inspector.Id)));
        }
    }
}