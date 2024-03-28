namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.GkhParam;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class PropertyOwnerProtocolsInterceptor : EmptyDomainInterceptor<PropertyOwnerProtocols>
    {
        public IDomainService<BasePropertyOwnerDecision> BasePropertyOwnerDecisionDomain { get; set; }
        public IDomainService<PropertyOwnerProtocolInspector> PropertyOwnerProtocolInspectorDomain { get; set; }
        public IDomainService<PropertyOwnerProtocolsDecision> PropertyOwnerProtocolsDecisionDomain { get; set; }
        public override IDataResult BeforeCreateAction(IDomainService<PropertyOwnerProtocols> service, PropertyOwnerProtocols entity)
        {
            var typeProtService = Container.Resolve<IDomainService<OwnerProtocolType>>();
            if (entity.ProtocolTypeId == null)
            {
                entity.ProtocolTypeId = typeProtService.GetAll().FirstOrDefault(x => x.Code == "1");
            }
            return Success();
           
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PropertyOwnerProtocols> service, PropertyOwnerProtocols entity)
        {
            BasePropertyOwnerDecisionDomain.GetAll().Where(x => x.PropertyOwnerProtocol.Id == entity.Id)
                            .Select(x => x.Id).ForEach(x => BasePropertyOwnerDecisionDomain.Delete(x));
            PropertyOwnerProtocolInspectorDomain.GetAll().Where(x => x.PropertyOwnerProtocols.Id == entity.Id)
                           .Select(x => x.Id).ForEach(x => PropertyOwnerProtocolInspectorDomain.Delete(x));
            PropertyOwnerProtocolsDecisionDomain.GetAll().Where(x => x.Protocol.Id == entity.Id)
                           .Select(x => x.Id).ForEach(x => PropertyOwnerProtocolsDecisionDomain.Delete(x));

            return Success();
        }

    }
}