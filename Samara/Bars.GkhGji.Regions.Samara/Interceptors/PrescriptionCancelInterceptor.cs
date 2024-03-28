namespace Bars.GkhGji.Regions.Samara.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class PrescriptionCancelInterceptor : GkhGji.Interceptors.PrescriptionCancelInterceptor
    {
        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<PrescriptionCancelViolReference> PrescriptionCancelViolRefDomain { get; set; }

        public IDomainService<InspectionGjiViolStage> InspectionViolStageDomain { get; set; }
        
        public override IDataResult BeforeDeleteAction(IDomainService<PrescriptionCancel> service, PrescriptionCancel entity)
        {
            var prescriptionCancelViols = this.PrescriptionCancelViolRefDomain.GetAll()
                .Where(x => x.PrescriptionCancel.Id == entity.Id).ToList();

            foreach (var item in prescriptionCancelViols)
            {
                var viol = this.InspectionViolDomain.Load(item.InspectionViol.InspectionViolation.Id);

                viol.DateCancel = null;
                this.InspectionViolDomain.Update(viol);
            }

            prescriptionCancelViols.ForEach(x => this.PrescriptionCancelViolRefDomain.Delete(x.Id));

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<PrescriptionCancel> service, PrescriptionCancel entity)
        {
            if (entity.TypeCancel == TypePrescriptionCancel.CompletelyCancel)
            {
                this.AddPrescriptionCancelViolRef(entity);
            }

            var violationStages = this.InspectionViolStageDomain.GetAll()
                .Where(x => x.Document.Id == entity.Prescription.Id).ToList();
            
            foreach (var item in violationStages)
            {
                var viol = this.InspectionViolDomain.Load(item.InspectionViolation.Id);
                viol.DateCancel = entity.DateCancel;
                this.InspectionViolDomain.Update(viol);
            }

            if (entity.Prolongation == TypeProlongation.Full)
            {
                this.UpdateDatePlanRemoval(entity);
            }

            this.CreateReminders(entity);

            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<PrescriptionCancel> service, PrescriptionCancel entity)
        {
            var prescriptionCancelViols = this.PrescriptionCancelViolRefDomain.GetAll()
                .Where(x => x.PrescriptionCancel.Id == entity.Id).ToList();

            foreach (var item in prescriptionCancelViols)
            {
                var viol = this.InspectionViolDomain.Load(item.InspectionViol.InspectionViolation.Id);
                viol.DateCancel = entity.DateCancel;
                this.InspectionViolDomain.Update(viol);
            }

            if (entity.TypeCancel == TypePrescriptionCancel.CompletelyCancel)
            {
                this.AddPrescriptionCancelViolRef(entity);
            }
            else if (entity.TypeCancel == TypePrescriptionCancel.Upheld)
            {
                prescriptionCancelViols.ForEach(x => this.PrescriptionCancelViolRefDomain.Delete(x.Id));
            }

            if (entity.Prolongation == TypeProlongation.Full)
            {
                this.UpdateDatePlanRemoval(entity);
            }
            else if (entity.Prolongation == TypeProlongation.Partly)
            {
                var presCancelViolRefs = this.PrescriptionCancelViolRefDomain.GetAll()
                    .Where(x => x.PrescriptionCancel.Id == entity.Id).ToList();

                foreach (var cancelViolRef in presCancelViolRefs)
                {
                    if (cancelViolRef.NewDatePlanRemoval != null)
                    {
                        var viol = this.InspectionViolDomain.Load(cancelViolRef.InspectionViol.InspectionViolation.Id);
                        viol.DatePlanRemoval = cancelViolRef.NewDatePlanRemoval;
                        this.InspectionViolDomain.Update(viol);
                    }
                }
            }

            this.CreateReminders(entity);
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<PrescriptionCancel> service, PrescriptionCancel entity)
        {
            this.CreateReminders(entity);
            return this.Success();
        }

        /// <summary>
        /// После создания решения, если "Тип решения" равен "Отменено полностью",
        /// то добавляем к нему все нарушения из предписания.
        /// </summary>
        /// <param name="entity">Решение об отмене</param>
        private void AddPrescriptionCancelViolRef(PrescriptionCancel entity)
        {
            var violationStages = this.InspectionViolStageDomain.GetAll()
                .Where(x => x.Document.Id == entity.Prescription.Id).ToList();

            var exist = this.PrescriptionCancelViolRefDomain.GetAll()
                .Where(x => x.PrescriptionCancel.Id == entity.Id)
                .Select(x => x.InspectionViol.Id).ToList();

            foreach (var item in violationStages)
            {
                if (exist.Contains(item.Id))
                {
                    continue;
                }

                var prescriptonCancelViolRef = new PrescriptionCancelViolReference
                {
                    PrescriptionCancel = entity,
                    InspectionViol = item
                };
                this.PrescriptionCancelViolRefDomain.Save(prescriptonCancelViolRef);
            }
        }

        /// <summary>
        /// Меняет для нарушений в акте проверки и предписании значения в столбце «Срок устранения» на Дату из поля «Продлить до»
        /// </summary>
        /// <param name="entity">
        /// Решение об отмене.
        /// </param>
        private void UpdateDatePlanRemoval(PrescriptionCancel entity)
        {
            var violationStages = this.InspectionViolStageDomain.GetAll()
                .Where(x => x.Document.Id == entity.Prescription.Id).ToList();

            foreach (var item in violationStages)
            {
                var viol = this.InspectionViolDomain.Load(item.InspectionViolation.Id);
                viol.DatePlanRemoval = entity.DateProlongation;
                this.InspectionViolDomain.Update(viol);
            }
        }

        private void CreateReminders(PrescriptionCancel entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var servReminderRule = Container.ResolveAll<IReminderRule>();

            try
            {
                var rule = servReminderRule.FirstOrDefault(x => x.Id == "InspectionReminderRule");
                if (rule != null)
                {
                    rule.Create(entity.Prescription);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(servReminderRule);
            }
        }
    }
}
