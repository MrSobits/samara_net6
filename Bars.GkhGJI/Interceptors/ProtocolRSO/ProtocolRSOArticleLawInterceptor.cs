namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ProtocolRSOArticleLawInterceptor : EmptyDomainInterceptor<ProtocolRSOArticleLaw>
    {
        public override IDataResult AfterCreateAction(IDomainService<ProtocolRSOArticleLaw> service, ProtocolRSOArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolRSO.Id.ToString() + " " + entity.ArticleLaw.Name);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<ProtocolRSOArticleLaw> service, ProtocolRSOArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolRSO.DocumentNumber + " " + entity.ArticleLaw.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ProtocolRSOArticleLaw> service, ProtocolRSOArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolRSO.DocumentNumber + " " + entity.ArticleLaw.Name);
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
                { "ProtocolRSO", "Протокол прокуратуры" },
                { "ArticleLaw", "Статья закона" },
                { "Description", "Примечание" }
            };
            return result;
        }
    }
}