namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;

    public class RealityObjectDecisionProtocolService : IRealityObjectDecisionProtocolService
    {
        private readonly IDomainService<RealityObjectDecisionProtocol> _domainService;

        public RealityObjectDecisionProtocolService(IDomainService<RealityObjectDecisionProtocol> domainService)
        {
            _domainService = domainService;
        }

        public RealityObjectDecisionProtocol GetActiveProtocol(RealityObject realityObject)
        {
            return GetActiveProtocolForDate(realityObject, DateTime.Now);
        }

        public RealityObjectDecisionProtocol GetActiveProtocolForDate(RealityObject realityObject, DateTime date)
        {
            return
                _domainService.GetAll()
                    .OrderByDescending(x => x.ProtocolDate)
                    .Where(x => x.State.FinalState)
                    .Where(x => x.RealityObject.Id == realityObject.Id)
                    .FirstOrDefault(x => x.DateStart <= date);
        }

        public void SetNextActualProtocol<T>(T entity) where T : IDecisionProtocol
        {
        }
    }
}
