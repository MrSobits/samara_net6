namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using System.Collections.Generic;

    /*
     * Переопределяется в регионах
     */
    public class PrescriptionCancelInterceptor : EmptyDomainInterceptor<PrescriptionCancel>
    {
        public override IDataResult AfterCreateAction(IDomainService<PrescriptionCancel> service, PrescriptionCancel entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.PrescriptionCancel, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<PrescriptionCancel> service, PrescriptionCancel entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.PrescriptionCancel, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.DocumentNum);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<PrescriptionCancel> service, PrescriptionCancel entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.PrescriptionCancel, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.DocumentNum);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "PrescriptionCancel", "Решение об отмене в предписании ГЖИ" },
                { "InspectionViol", "Этап указания к устранению нарушения в предписании" },
                { "NewDatePlanRemoval", "Новый срок устранения" }
            };
            return result;
        }
    }
}
