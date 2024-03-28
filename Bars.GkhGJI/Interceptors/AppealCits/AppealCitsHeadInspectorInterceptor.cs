namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Entities;
    using System.Collections.Generic;

    public class AppealCitsHeadInspectorInterceptor : EmptyDomainInterceptor<AppealCitsHeadInspector>
    {
        public override IDataResult AfterCreateAction(IDomainService<AppealCitsHeadInspector> service, AppealCitsHeadInspector entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            var inspDomain = Container.ResolveDomain<Inspector>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsHeadInspector, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + inspDomain.Get(entity.Inspector.Id).Fio);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsHeadInspector> service, AppealCitsHeadInspector entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCitsHeadInspector, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.Inspector.Fio);
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
                { "Inspector", "Инспектор" }
            };
            return result;
        }

    }
}
