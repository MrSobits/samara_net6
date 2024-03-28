namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Linq;
    using B4;
    using Castle.Windsor;
    using Entities;

    public class OwnerProtocolTypeDecisionsService : IOwnerProtocolTypeDecisionsService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<OwnerProtocolTypeDecision> ProtocolTypeDecisionDomain { get; set; }

        public IDomainService<PropertyOwnerProtocols> ProtocolDomain { get; set; }

        public IDataResult SelectDecision(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var protocolId = baseParams.Params.GetAs<long>("protocolId");

            long ownerProtId = ProtocolDomain.GetAll()
                .Where(x => x.Id == protocolId)
                .Select(x => x.ProtocolTypeId.Id)
                .FirstOrDefault();

            if (ownerProtId > 0)
            {
                var data = ProtocolTypeDecisionDomain.GetAll()
                 .Where(x => x.OwnerProtocolType.Id == ownerProtId)
                 .Select(x => new
                 {
                     x.Id,
                     x.Name
                 })
                 .OrderBy(x => x.Name)
                 .Filter(loadParams, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            else
            {
                var data = ProtocolTypeDecisionDomain.GetAll()                  
                    .Select(x => new
                    {
                        x.Id,
                        x.Name
                    })
                    .OrderBy(x => x.Name)
                    .Filter(loadParams, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
        }
    }
}