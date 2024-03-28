namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;
    using B4;

    using Bars.GkhGji.Enums;

    using Entities;

    /// <summary>
    /// Интерцептор сущности "Этап указания к устранению нарушения в предписании" (см. <see cref="PrescriptionViol"/>)
    /// </summary>
    public class PrescriptionViolInterceptor : EmptyDomainInterceptor<PrescriptionViol>
    {
        /// <summary>
        /// Домент-сервис "Этап нарушения"
        /// </summary>
        public IDomainService<InspectionGjiViolStage> ViolStageDomain { get; set; }

        /// <summary>
        /// Домент-сервис Нарушение проверки
        /// </summary>
        public IDomainService<InspectionGjiViol> ViolDomain { get; set; }

        /// <summary>
        /// Домент-сервис сущности "Таблица связи документов (Какой документ из какого был сформирован)"
        /// </summary>
        public IDomainService<DocumentGjiChildren> DocChildrenDomain { get; set; }

        /// <summary>
        /// Домент-сервис "Решение об отмене в предписании ГЖИ"
        /// </summary>
        public IDomainService<PrescriptionCancel> PrescriptionCancelDomain { get; set; }

        /// <summary>
        /// Домент-сервис "Сущность связи Решения и Нарушения."
        /// </summary>
        public IDomainService<PrescriptionCancelViolReference> PrescriptionCancelViolRefDomain { get; set; }

        /// <summary>
        /// Домент-сервис "Таблица связи документов (Какой документ из какого был сформирован)"
        /// </summary>
        public IDomainService<DocumentGjiChildren> DocumentChildrenDomain { get; set; }

        /// <summary>
        /// Домент-сервис "Этап устранения нарушения в акте проверки"
        /// </summary>
        public IDomainService<ActRemovalViolation> ActRemViolDomain { get; set; }


        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен сервис сущности <see cref="PrescriptionViol"/></param>
        /// <param name="entity">Сущность <see cref="PrescriptionViol"/></param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<PrescriptionViol> service, PrescriptionViol entity)
        {
            var datePlanRemovalStageMax = this.ViolStageDomain.GetAll()
               .Where(x => x.InspectionViolation.Inspection.Id == entity.InspectionViolation.Id)
               .Max(x => x.DatePlanRemoval);

            var viol = this.ViolDomain.Get(entity.InspectionViolation.Id);
            if (viol != null)
            {
                viol.DatePlanRemoval = datePlanRemovalStageMax.HasValue
                        ? datePlanRemovalStageMax > entity.DatePlanRemoval
                            ? datePlanRemovalStageMax
                            : entity.DatePlanRemoval
                        : null;

                viol.DateFactRemoval = entity.DateFactRemoval;

                if (!string.IsNullOrEmpty(entity.Description))
                {
                    viol.Description = entity.Description;
                }

                if (!string.IsNullOrEmpty(entity.Action))
                {
                    viol.Action = entity.Action;
                }

                this.ViolDomain.Update(viol);
            }

            var docChildrenParent = this.DocChildrenDomain.GetAll().FirstOrDefault(x => x.Children.Id == entity.Document.Id);
            if (docChildrenParent != null)
            {
                var actCheckDate = docChildrenParent.Parent.DocumentDate;
                var result = this.CheckDatePlanRemoval(entity, actCheckDate);
                if (!result.Success)
                {
                    return result;
                }
            }

            return this.Success();
        }

        /// <summary>
        /// Действие, выполняемое до создания сущности
        /// </summary>
        /// <param name="service">Домен сервис сущности <see cref="PrescriptionViol"/></param>
        /// <param name="entity">Сущность <see cref="PrescriptionViol"/></param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterCreateAction(IDomainService<PrescriptionViol> service, PrescriptionViol entity)
        {
            var prescriptionCancels = this.PrescriptionCancelDomain.GetAll().
                Where(x => x.Prescription.Id == entity.Document.Id).ToList();

            foreach (var prescriptionCancel in prescriptionCancels)
            {
                // Если тип отмены у решения "Отменено полностью", то добавляем нарушение к отмененным нарушениям вкладки решения
                if (prescriptionCancel.TypeCancel == TypePrescriptionCancel.CompletelyCancel)
                {
                    var prescriptionCancelViolRef = new PrescriptionCancelViolReference
                    {
                        PrescriptionCancel = prescriptionCancel,
                        InspectionViol = entity
                    };
                    this.PrescriptionCancelViolRefDomain.Save(prescriptionCancelViolRef);
                }
            }

            return this.Success();
        }

        /// <summary>
        /// Действие, выполняемое до создания сущности
        /// </summary>
        /// <param name="service">Домен сервис сущности <see cref="PrescriptionViol"/></param>
        /// <param name="entity">Сущность <see cref="PrescriptionViol"/></param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<PrescriptionViol> service, PrescriptionViol entity)
        {
            if (entity.InspectionViolation != null)
            {
                entity.Description = entity.InspectionViolation.Violation.Name;
            }

            return this.Success();
        }

        /// <summary>
        /// Действие, выполняемое после обновления сущности
        /// </summary>
        /// <param name="service">Домен сервис сущности <see cref="PrescriptionViol"/></param>
        /// <param name="entity">Сущность <see cref="PrescriptionViol"/></param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterUpdateAction(IDomainService<PrescriptionViol> service, PrescriptionViol entity)
        {

            var violActRemoval = this.ActRemViolDomain.GetAll()
                .Where(y => this.DocumentChildrenDomain.GetAll().Any(x => x.Children.Id == y.Document.Id && x.Parent.Id == entity.Document.Id)
                    && y.InspectionViolation.Id == entity.InspectionViolation.Id)
                .ToArray();

            foreach (var viol in violActRemoval)
            {
                viol.DatePlanRemoval = entity.DatePlanRemoval;
                this.ActRemViolDomain.Update(viol);
            }

            var serviceProtViol = this.Container.Resolve<IDomainService<ProtocolViolation>>();

            var violProtocol = serviceProtViol.GetAll()
                .Where(y => this.DocumentChildrenDomain.GetAll().Any(x => x.Children.Id == y.Document.Id && x.Parent.Id == entity.Document.Id)
                    && y.InspectionViolation.Id == entity.InspectionViolation.Id)
                .ToArray();

            foreach (var viol in violProtocol)
            {
                viol.DatePlanRemoval = entity.DatePlanRemoval;
                serviceProtViol.Update(viol);
            }

            return this.Success();
        }

        /// <summary>
        /// Проверка плановой даты устранения (переопределяемый)
        /// </summary>
        /// <param name="entity">Сущность <see cref="PrescriptionViol"/></param>
        /// <param name="actCheckDate">Дата родительского документа</param>
        /// <returns>Результат выполнения проверки</returns>
        protected virtual IDataResult CheckDatePlanRemoval(PrescriptionViol entity, DateTime? actCheckDate)
        {
            if (entity.DatePlanRemoval.HasValue && actCheckDate.HasValue && entity.DatePlanRemoval.Value > actCheckDate.Value.AddMonths(24))
            {
                return this.Failure("Срок устранения нарушения не должен превышать 6 месяцев.");
            }

            return this.Success();
        }
    }
}