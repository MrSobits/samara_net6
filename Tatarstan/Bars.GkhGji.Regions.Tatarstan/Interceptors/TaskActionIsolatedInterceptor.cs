namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using NHibernate.Linq;

    public class TaskActionIsolatedInterceptor : DocumentGjiInterceptor<TaskActionIsolated>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<TaskActionIsolated> service, TaskActionIsolated entity)
        {
            IDomainService<InspectionGji> domainInspection;
            IDomainService<InspectionGjiStage> domainStage;

            using (this.Container.Using(domainStage = this.Container.Resolve<IDomainService<InspectionGjiStage>>(),
                domainInspection = this.Container.Resolve<IDomainService<InspectionGji>>()))
            {
                var newInspection = new InspectionGji
                {
                    TypeBase = TypeBase.ActionIsolated
                };

                domainInspection.Save(newInspection);

                var newStage = new InspectionGjiStage
                {
                    Inspection = newInspection,
                    TypeStage = TypeStage.TaskActionIsolated,
                    Position = 0
                };

                domainStage.Save(newStage);

                entity.Inspection = newInspection;
                entity.Stage = newStage;

                if (entity.DocumentDate.HasValue)
                {
                    entity.DocumentYear = entity.DocumentDate.Value.Year;
                }

                return base.BeforeCreateAction(service, entity);
            }
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<TaskActionIsolated> service, TaskActionIsolated entity)
        {
            var inspectionDomain = this.Container.Resolve<IDomainService<InspectionGji>>();
            var documentDomain = this.Container.Resolve<IDomainService<DocumentGji>>();

            using (this.Container.Using(inspectionDomain, documentDomain))
            {
                // Сначала вызываем удаление базового потому что там произходит зачистка Stage 
                var result = base.AfterDeleteAction(service, entity);
                var inspectionId = entity.Inspection.Id;
                if (inspectionId > 0)
                {
                    if (!documentDomain.GetAll().Any(x => x.Inspection.Id == inspectionId))
                    {
                        inspectionDomain.Delete(inspectionId);
                    }
                }

                return result;
            }
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<TaskActionIsolated> service, TaskActionIsolated entity)
        {
            var taskKnmActionDomain = this.Container.ResolveDomain<TaskActionIsolatedKnmAction>();
            var taskSurveyPurposeDomain = this.Container.ResolveDomain<TaskActionIsolatedSurveyPurpose>();
            var taskRoDomain = this.Container.ResolveDomain<TaskActionIsolatedRealityObject>();
            var taskAnnexDomain = this.Container.ResolveDomain<TaskActionIsolatedAnnex>();
            var taskArticleLawDomain = this.Container.ResolveDomain<TaskActionIsolatedArticleLaw>();
            var taskItemDomain = this.Container.ResolveDomain<TaskActionIsolatedItem>();

            using (this.Container.Using(taskKnmActionDomain,
                       taskSurveyPurposeDomain,
                       taskRoDomain,
                       taskAnnexDomain,
                       taskArticleLawDomain,
                       taskItemDomain))
            {
                taskKnmActionDomain.GetAll()
                    .Where(x => x.MainEntity.Id == entity.Id)
                    .Delete();

                taskSurveyPurposeDomain.GetAll()
                    .Where(x => x.TaskActionIsolated.Id == entity.Id)
                    .Delete();

                taskRoDomain.GetAll()
                    .Where(x => x.Task.Id == entity.Id)
                    .Delete();
                
                taskAnnexDomain.GetAll()
                    .Where(x => x.Task.Id == entity.Id)
                    .Delete();
                
                taskArticleLawDomain.GetAll()
                    .Where(x => x.Task.Id == entity.Id)
                    .Delete();
                
                taskItemDomain.GetAll()
                    .Where(x => x.Task.Id == entity.Id)
                    .Delete();
            }
            
            return base.BeforeDeleteAction(service, entity);
        }
    }
}
