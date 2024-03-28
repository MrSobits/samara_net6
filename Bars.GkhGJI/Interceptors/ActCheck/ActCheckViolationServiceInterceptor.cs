namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;

    using Bars.GkhGji.Utils;
    using B4;
    using Entities;

    public class ActCheckViolationServiceInterceptor : EmptyDomainInterceptor<ActCheckViolation>
    {
        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<DisposalViolation> DisposalViolDomain { get; set; }

        public IDomainService<InspectionGjiViolStage> InspectionViolStageDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<ActCheckViolation> service, ActCheckViolation entity)
        {
            var datePlanRemovalStageMax = InspectionViolStageDomain.GetAll()
                    .Where(x => x.InspectionViolation.Inspection.Id == entity.InspectionViolation.Id)
                    .Max(x => x.DatePlanRemoval);

            // Перед обновлением обновляем Дату начала проверки в самом нарушении проверки
            // Поскольку ActCheckGjiViolation это только этап нарушения а само нарушение это InspectionGjiViolation
            var viol = InspectionViolDomain.Load(entity.InspectionViolation.Id);
            viol.DatePlanRemoval = datePlanRemovalStageMax > entity.DatePlanRemoval ? datePlanRemovalStageMax : entity.DatePlanRemoval;
            InspectionViolDomain.Update(viol);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ActCheckViolation> service, ActCheckViolation entity)
        {
            var docs =
                InspectionViolStageDomain.GetAll()
                                         // В случае если нарушение есть в приказе (напрмиер как в Томске), т оможно удалят ьнарушение из акта поскольку акт не является начальным документов для нарушений
                                         .Where(x => !DisposalViolDomain.GetAll().Any(y => y.InspectionViolation.Id == entity.InspectionViolation.Id))
                                         .Where(
                                             x =>
                                             x.Document.Id != entity.ActObject.ActCheck.Id
                                             && x.InspectionViolation.Id == entity.InspectionViolation.Id)
                                         .Select(
                                             x =>
                                             new
                                                 {
                                                     x.Document.Id,
                                                     x.Document.TypeDocumentGji,
                                                     x.Document.DocumentNumber,
                                                     x.Document.DocumentDate
                                                 })
                                         .AsEnumerable()
                                         .Select(
                                             x =>
                                             Utils.GetDocumentName(x.TypeDocumentGji) + " №" + x.DocumentNumber
                                             + (x.DocumentDate.HasValue ? " от "+x.DocumentDate.Value.ToShortDateString() : string.Empty))
                                         .ToList();
            if (docs.Any())
            {
                var strDocs = docs.Aggregate((x, y) => !string.IsNullOrEmpty(y) ? y + ", " + x: x );
                var violationName = entity.InspectionViolation?.Violation?.Name;
                var violName = violationName.Length > 100 ? violationName.Substring(0, 99) + "..." : violationName;

                throw new ValidationException($"Нарушение \"{violName}\" содержится в связанных документах: {strDocs}");
            }

            // Перед удалением загружаем
            entity.InspectionViolation = InspectionViolDomain.Load(entity.InspectionViolation.Id);

            return Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActCheckViolation> service, ActCheckViolation entity)
        {
            // Если на базовое нарушение больше нет ссылок то тоже удаляем данное нарушение
            if (!InspectionViolStageDomain.GetAll().Any(x => x.Id != entity.Id && x.InspectionViolation.Id == entity.InspectionViolation.Id))
            {
                InspectionViolDomain.Delete(entity.InspectionViolation.Id);
            }

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActCheckViolation> service, ActCheckViolation entity)
        {
            var serviceDocumentChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceActRemViol = Container.Resolve<IDomainService<PrescriptionViol>>();
            var serviceProtViol = Container.Resolve<IDomainService<ProtocolViolation>>();

            try
            {
                var violPrescription = serviceActRemViol.GetAll()
                .Where(y => serviceDocumentChildren.GetAll().Any(x => x.Children.Id == y.Document.Id && x.Parent.Id == entity.Document.Id)
                    && y.InspectionViolation.Id == entity.InspectionViolation.Id)
                .ToArray();

                foreach (var viol in violPrescription)
                {
                    viol.DatePlanRemoval = entity.DatePlanRemoval;
                    serviceActRemViol.Update(viol);
                }

                var violProtocol = serviceProtViol.GetAll()
                    .Where(y => serviceDocumentChildren.GetAll().Any(x => x.Children.Id == y.Document.Id && x.Parent.Id == entity.Document.Id)
                        && y.InspectionViolation.Id == entity.InspectionViolation.Id)
                    .ToArray();

                foreach (var viol in violProtocol)
                {
                    viol.DatePlanRemoval = entity.DatePlanRemoval;
                    serviceProtViol.Update(viol);
                }

                return Success();
            }
            finally 
            {
                Container.Release(serviceDocumentChildren);
                Container.Release(serviceActRemViol);
                Container.Release(serviceProtViol);
            }
        }
    }
}