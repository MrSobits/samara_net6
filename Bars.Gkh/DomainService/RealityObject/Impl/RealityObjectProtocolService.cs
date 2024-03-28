namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Castle.Windsor;

    public class RealityObjectProtocolService : IRealityObjectProtocolService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetProtocolByRealityObjectId(BaseParams baseParams)
        {
            var realtyObjectId = baseParams.Params.GetAsId("realtyObjectId");

            var protocol = 
                Container.Resolve<IDomainService<RealityObjectProtocol>>().GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == realtyObjectId);

            var id = 0L;
            if (protocol != null)
            {
                id = protocol.Id;
            }

            return new BaseDataResult(new {protocolId = id});
        }
    }
}