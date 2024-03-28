namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    public class ProtocolBaseDocumentInterceptor : EmptyDomainInterceptor<ProtocolBaseDocument>
    {
        public override IDataResult AfterCreateAction(IDomainService<ProtocolBaseDocument> service, ProtocolBaseDocument entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiBaseDocument, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ProtocolBaseDocument> service, ProtocolBaseDocument entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiBaseDocument, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.DocumentNumber + entity.NumDoc);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ProtocolBaseDocument> service, ProtocolBaseDocument entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiBaseDocument, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.DocumentNumber + entity.NumDoc);
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
                { "Protocol", "Протокол" },
                { "RealityObject", "МКД" },
                { "KindBaseDocument", "Направление деятельности субъекта проверки" },
                { "DateDoc", "Дата документа" },
                { "NumDoc", "Номер документа" }
            };
            return result;
        }
    }
}