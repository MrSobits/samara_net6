namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckProvidedDocInterceptor : EmptyDomainInterceptor<ActCheckProvidedDoc>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActCheckProvidedDoc> service, ActCheckProvidedDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiProvidedDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.Id.ToString() + " " + entity.ProvidedDoc.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActCheckProvidedDoc> service, ActCheckProvidedDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiProvidedDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.ProvidedDoc.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActCheckProvidedDoc> service, ActCheckProvidedDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiProvidedDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.ProvidedDoc.Name);
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
                { "ProvidedDoc", "Предоставляемый документа" },
                { "DateProvided", "Дата предосталвения" },
                { "ActCheck", "Факт проверки" }
            };
            return result;
        }
    }
}