namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ProtocolMvdArticleLawInterceptor : EmptyDomainInterceptor<ProtocolMvdArticleLaw>
    {
        public override IDataResult AfterCreateAction(IDomainService<ProtocolMvdArticleLaw> service, ProtocolMvdArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolMvd.Id.ToString() + " " + entity.ArticleLaw.Name);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<ProtocolMvdArticleLaw> service, ProtocolMvdArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolMvd.DocumentNumber + " " + entity.ArticleLaw.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ProtocolMvdArticleLaw> service, ProtocolMvdArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolMvd.DocumentNumber + " " + entity.ArticleLaw.Name);
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
                { "ProtocolMvd", "Протокол МВД" },
                { "ArticleLaw", "Статья закона" },
                { "Description", "Примечание" }
            };
            return result;
        }
    }
}