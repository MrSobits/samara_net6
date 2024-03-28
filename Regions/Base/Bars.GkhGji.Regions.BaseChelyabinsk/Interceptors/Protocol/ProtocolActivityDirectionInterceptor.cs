namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    public class ProtocolActivityDirectionInterceptor : EmptyDomainInterceptor<ProtocolActivityDirection>
    {
        public override IDataResult AfterCreateAction(IDomainService<ProtocolActivityDirection> service, ProtocolActivityDirection entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiActivityDirection, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.Id.ToString() + " " + entity.ActivityDirection.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ProtocolActivityDirection> service, ProtocolActivityDirection entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiActivityDirection, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.DocumentNumber + entity.ActivityDirection.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ProtocolActivityDirection> service, ProtocolActivityDirection entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiActivityDirection, entity.Id, entity.GetType(), GetPropertyValues(), entity.Protocol.DocumentNumber + entity.ActivityDirection.Name);
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
                { "Protocol", "Протокол" }
            };
            return result;
        }
    }
}