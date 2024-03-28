namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.InspectionActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;

    public class InspectionActionIsolatedInterceptor: InspectionGjiInterceptor<InspectionActionIsolated>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<InspectionActionIsolated> service, InspectionActionIsolated entity)
        {
            entity.TypeBase = TypeBase.InspectionActionIsolated;

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<InspectionActionIsolated> service, InspectionActionIsolated entity)
        {
            var inspectionRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            
            var taskRo = Container.Resolve<IDomainService<TaskActionIsolatedRealityObject>>();

            using (this.Container.Using(inspectionRo, taskRo))
            {
                var taskRealityObjects = taskRo.GetAll().Where(x => x.Task.Inspection.Id == entity.ActionIsolated.Id).ToList();

                foreach (var taskRealityObject in taskRealityObjects)
                {
                    var inspectionObject = new InspectionGjiRealityObject
                    {
                        Inspection = entity,
                        RealityObject = taskRealityObject.RealityObject
                    };

                    inspectionRo.Save(inspectionObject);
                }
            }

            return base.AfterCreateAction(service, entity);
        }
    }
}