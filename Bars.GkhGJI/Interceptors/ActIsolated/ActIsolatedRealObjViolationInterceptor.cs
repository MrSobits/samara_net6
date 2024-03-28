namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActIsolatedRealObjViolationInterceptor : EmptyDomainInterceptor<ActIsolatedRealObjViolation>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActIsolatedRealObjViolation> service, ActIsolatedRealObjViolation entity)
        {
            var serviceInspectionViol = this.Container.Resolve<IDomainService<InspectionGjiViol>>();            
            try
            {
                var newViol = new InspectionGjiViol
                {
                    Inspection = new InspectionGji
                    {
                        Id = entity.ActIsolatedRealObj.ActIsolated.Inspection.Id
                    },
                    RealityObject = entity.ActIsolatedRealObj.RealityObject,
                    Violation = new ViolationGji
                    {
                        Id = entity.ViolationGjiId
                    }
                };
                serviceInspectionViol.Save(newViol);

                entity.InspectionViolation = newViol;
                entity.TypeViolationStage = TypeViolationStage.Detection;
                entity.Document = entity.ActIsolatedRealObj.ActIsolated;

                return base.BeforeCreateAction(service, entity);
            }
            finally
            {
                this.Container.Release(serviceInspectionViol);
            }
        }
    }
}