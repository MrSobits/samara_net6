namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ProtocolMhcArticleLawInterceptor : EmptyDomainInterceptor<ProtocolMhcArticleLaw>
    {
        public override IDataResult AfterCreateAction(IDomainService<ProtocolMhcArticleLaw> service, ProtocolMhcArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolMhc.Id.ToString() + " " + entity.ArticleLaw.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ProtocolMhcArticleLaw> service, ProtocolMhcArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolMhc.DocumentNumber + " " + entity.ArticleLaw.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ProtocolMhcArticleLaw> service, ProtocolMhcArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolMhc.DocumentNumber + " " + entity.ArticleLaw.Name);
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
                { "ProtocolMhc", "Протокол МЖК" },
                { "ArticleLaw", "Статья закона" },
                { "Description", "Примечание" }
            };
            return result;
        }
    }
}