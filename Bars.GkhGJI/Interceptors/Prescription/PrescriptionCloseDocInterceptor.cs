namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class PrescriptionCloseDocInterceptor : EmptyDomainInterceptor<PrescriptionCloseDoc>
    {
        public override IDataResult AfterCreateAction(IDomainService<PrescriptionCloseDoc> service, PrescriptionCloseDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.PrescriptionCloseDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.Id.ToString() + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<PrescriptionCloseDoc> service, PrescriptionCloseDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.PrescriptionCloseDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<PrescriptionCloseDoc> service, PrescriptionCloseDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.PrescriptionCloseDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.Name);
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
                { "Prescription", "Предписание" },
                { "DocType", "Тип документа" },
                { "Date", "Дата предоставления" },
                { "Name", "Наименование" },
                { "File", "Файл" }
            };
            return result;
        }
    }
}