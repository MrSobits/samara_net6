namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197ArticleLawInterceptor : EmptyDomainInterceptor<Protocol197ArticleLaw>
    {
        public override IDataResult AfterCreateAction(IDomainService<Protocol197ArticleLaw> service, Protocol197ArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol197.Id.ToString() + " " + entity.ArticleLaw.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<Protocol197ArticleLaw> service, Protocol197ArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol197.DocumentNumber + " " + entity.ArticleLaw.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<Protocol197ArticleLaw> service, Protocol197ArticleLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol197.DocumentNumber + " " + entity.ArticleLaw.Name);
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
                { "Protocol197", "Протокол 19.7" },
                { "ArticleLaw", "Статья закона" },
                { "Description", "Примечание" }
            };
            return result;
        }
    }
}