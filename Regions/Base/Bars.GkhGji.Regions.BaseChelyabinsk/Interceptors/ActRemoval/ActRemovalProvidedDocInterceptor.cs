namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ActRemovalProvidedDocInterceptor : EmptyDomainInterceptor<ActRemovalProvidedDoc>
    {
        public override IDataResult AfterCreateAction(IDomainService<ActRemovalProvidedDoc> service, ActRemovalProvidedDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiProvidedDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.Id.ToString() + " " + entity.ProvidedDoc.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActRemovalProvidedDoc> service, ActRemovalProvidedDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiProvidedDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.ProvidedDoc.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActRemovalProvidedDoc> service, ActRemovalProvidedDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiProvidedDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.ProvidedDoc.Name);
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
                { "ActRemoval", "Aкт проверки предписания" }
            };
            return result;
        }
    }
}