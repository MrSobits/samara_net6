namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using Enums;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;

    class ResolutionArtLawInterceptor : EmptyDomainInterceptor<ResolutionArtLaw>
    {
        public override IDataResult AfterCreateAction(IDomainService<ResolutionArtLaw> service, ResolutionArtLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.Id.ToString() + " " + entity.ArticleLawGji.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ResolutionArtLaw> service, ResolutionArtLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.ArticleLawGji.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ResolutionArtLaw> service, ResolutionArtLaw entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiArticleLaw, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.ArticleLawGji.Name);
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
                { "Resolution", "Постановление" },
                { "DocumentDate", "Дата изменения статьи закона" },
                { "Description", "Описание" },
                { "ArticleLawGji", "Статья закона" }
            };
            return result;
        }
    }
}
