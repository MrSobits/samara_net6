namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    using Gkh.Authentification;

    public class ViewBaseStatementDomainService : ViewBaseStatementDomainService<ViewBaseStatement>
    {
        public override IQueryable<ViewBaseStatement> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var serviceInspectionInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var serviceInspectionAppeal= Container.Resolve<IDomainService<InspectionAppealCits>>();

            try
            {
                var inspectorIds = userManager.GetInspectorIds();
                var municipalityList = userManager.GetMunicipalityIds(); // В томске попросили убрать ограничение по МО

                return base.GetAll()
                        .WhereIf(inspectorIds.Count == 0 && municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value))
                        .WhereIf(inspectorIds.Count > 0, y => 
                            serviceInspectionInspector.GetAll().Any(x => x.Inspection.Id == y.Id && inspectorIds.Contains(x.Inspector.Id))
                         || serviceInspectionAppeal.GetAll()
                              .Any(x => x.Inspection.Id == y.Id 
                                        && (inspectorIds.Contains(x.AppealCits.Surety.Id)
                                            || inspectorIds.Contains(x.AppealCits.Executant.Id)
                                            || inspectorIds.Contains(x.AppealCits.Tester.Id))
                                )
                         );
            }
            finally 
            {
                Container.Release(userManager);
                Container.Release(serviceInspectionInspector);
            }
        }
    }
}