namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197ActivityDirectionInterceptor : EmptyDomainInterceptor<Protocol197ActivityDirection>
    {
        public override IDataResult AfterCreateAction(IDomainService<Protocol197ActivityDirection> service, Protocol197ActivityDirection entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiActivityDirection, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol197.Id.ToString() + " " + entity.ActivityDirection.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<Protocol197ActivityDirection> service, Protocol197ActivityDirection entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiActivityDirection, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol197.DocumentNumber + " " + entity.ActivityDirection.Name);
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
                { "ActivityDirection", "Направление деятельности субъекта првоерки" },
                { "Protocol197", "Протокол" }
            };
            return result;
        }
    }
}