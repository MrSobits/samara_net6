namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.AppealCits
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;

    /// <summary>
    /// Interceptor для <see cref="MotivatedPresentationAppealCits"/>
    /// </summary>
    public class MotivatedPresentationAppealCitsInterceptor : DocumentGjiInterceptor<MotivatedPresentationAppealCits>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<MotivatedPresentationAppealCits> service, MotivatedPresentationAppealCits entity)
        {
            if (service.GetAll().Any(x => x.AppealCits.Id == entity.AppealCits.Id))
                throw new ValidationException("По текущему обращению уже создан документ \"Мотивированное представление\"");

            var inspectionGjiDomain = this.Container.ResolveDomain<InspectionGji>();
            var inspectionGjiStageDomain = this.Container.ResolveDomain<InspectionGjiStage>();

            using (this.Container.Using(inspectionGjiDomain, inspectionGjiStageDomain))
            {
                var newInspection = new InspectionGji
                {
                    TypeBase = TypeBase.MotivatedPresentationAppealCits
                };

                inspectionGjiDomain.Save(newInspection);

                var newStage = new InspectionGjiStage
                {
                    Inspection = newInspection,
                    TypeStage = TypeStage.MotivatedPresentationAppealCits
                };

                inspectionGjiStageDomain.Save(newStage);

                entity.Inspection = newInspection;
                entity.Stage = newStage;

                if (!entity.DocumentDate.HasValue)
                    entity.DocumentDate = DateTime.Now;

                entity.DocumentYear = entity.DocumentDate.Value.Year;

                return base.BeforeCreateAction(service, entity);
            }
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<MotivatedPresentationAppealCits> service, MotivatedPresentationAppealCits entity)
        {
            var inspectionDomain = this.Container.Resolve<IDomainService<InspectionGji>>();
            var documentDomain = this.Container.Resolve<IDomainService<DocumentGji>>();

            using (this.Container.Using(inspectionDomain, documentDomain))
            {
                // Удаление Stage 
                var result = base.AfterDeleteAction(service, entity);

                var inspectionId = entity.Inspection?.Id;
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
    }
}