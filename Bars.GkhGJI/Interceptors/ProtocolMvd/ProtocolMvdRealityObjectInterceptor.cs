namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ProtocolMvdRealityObjectInterceptor : EmptyDomainInterceptor<ProtocolMvdRealityObject>
    {
        public override IDataResult AfterCreateAction(IDomainService<ProtocolMvdRealityObject> service, ProtocolMvdRealityObject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            var roDomain = Container.ResolveDomain<RealityObject>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiRealityObject, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolMvd.Id.ToString() + " " + roDomain.Get(entity.RealityObject.Id).Address);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ProtocolMvdRealityObject> service, ProtocolMvdRealityObject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiRealityObject, entity.Id, entity.GetType(), GetPropertyValues(), entity.ProtocolMvd.DocumentNumber + " " + entity.RealityObject.Address);
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
                { "RealityObject", "Жилой дом" }
            };
            return result;
        }
    }
}