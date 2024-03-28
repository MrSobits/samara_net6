namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Entities;
    using System.Collections.Generic;

    public class AppealCitsCategoryInterceptor : EmptyDomainInterceptor<AppealCitsCategory>
    {
        public override IDataResult AfterCreateAction(IDomainService<AppealCitsCategory> service, AppealCitsCategory entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            var categoryDomain = Container.ResolveDomain<ApplicantCategory>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsCategory, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + categoryDomain.Get(entity.ApplicantCategory.Id).Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsCategory> service, AppealCitsCategory entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCitsCategory, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.ApplicantCategory.Name);
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
                { "AppealCits", "Обращение" },
                { "ApplicantCategory", "Категории заявителя" }
            };
            return result;
        }

    }
}
