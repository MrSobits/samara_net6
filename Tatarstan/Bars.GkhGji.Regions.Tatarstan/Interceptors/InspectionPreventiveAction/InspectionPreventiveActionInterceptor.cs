namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.InspectionPreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionPreventiveAction;

    /// <summary>
    /// Interceptor для <see cref="InspectionPreventiveAction"/>
    /// </summary>
    public class InspectionPreventiveActionInterceptor : InspectionGjiInterceptor<InspectionPreventiveAction>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<InspectionPreventiveAction> service, InspectionPreventiveAction entity)
        {
            entity.TypeBase = TypeBase.InspectionPreventiveAction;

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<InspectionPreventiveAction> service, InspectionPreventiveAction entity)
        {
            var inspectionGjiRealityObjectDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var motivatedPresentationRealityObjectDomain = this.Container.ResolveDomain<MotivatedPresentationRealityObject>();

            using (this.Container.Using(inspectionGjiRealityObjectDomain, motivatedPresentationRealityObjectDomain))
            {
                motivatedPresentationRealityObjectDomain.GetAll()
                    .Where(x => x.MotivatedPresentation.Inspection.Id == entity.PreventiveAction.Id)
                    .ToList()
                    .ForEach(x =>
                    {
                        var inspectionObject = new InspectionGjiRealityObject
                        {
                            Inspection = entity,
                            RealityObject = x.RealityObject
                        };

                        inspectionGjiRealityObjectDomain.Save(inspectionObject);
                    });
            }

            return base.AfterCreateAction(service, entity);
        }
    }
}