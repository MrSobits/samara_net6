namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class PrescriptionBaseDocumentInterceptor : EmptyDomainInterceptor<PrescriptionBaseDocument>
    {
        public override IDataResult AfterCreateAction(IDomainService<PrescriptionBaseDocument> service, PrescriptionBaseDocument entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiBaseDocument, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<PrescriptionBaseDocument> service, PrescriptionBaseDocument entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiBaseDocument, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.NumDoc);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<PrescriptionBaseDocument> service, PrescriptionBaseDocument entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiBaseDocument, entity.Id, entity.GetType(), GetPropertyValues(), entity.Prescription.DocumentNumber + " " + entity.NumDoc);
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
                { "KindBaseDocument", "Направление деятельности субъекта првоерки" },
                { "DateDoc", "Дата документа" },
                { "NumDoc", "Номер документа" }
            };
            return result;
        }
    }
}