namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DecisionProvidedDocInterceptor : EmptyDomainInterceptor<DecisionProvidedDoc>
    {
        public override IDataResult AfterCreateAction(IDomainService<DecisionProvidedDoc> service, DecisionProvidedDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiProvidedDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.Id.ToString() + " " + entity.ProvidedDoc.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<DecisionProvidedDoc> service, DecisionProvidedDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiProvidedDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.ProvidedDoc.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DecisionProvidedDoc> service, DecisionProvidedDoc entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiProvidedDoc, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.ProvidedDoc.Name);
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
                { "Description", "Примечание" },
                { "Decision", "Распоряжение" }
            };
            return result;
        }
    }
}