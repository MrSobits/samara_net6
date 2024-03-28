namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;
    using Entities;
    using Gkh.Entities;
    using Castle.Windsor;

    public class UltimateDecisionService : IUltimateDecisionService
    {
        private readonly IWindsorContainer _container;

        public UltimateDecisionService(IWindsorContainer container)
        {
            _container = container;
        }

        public T GetActualDecision<T>(RealityObject realityObject) where T : UltimateDecision
        {
            return GetActualDecision<T>(realityObject.Id);
        }

        public T GetActualDecision<T>(long realityObjId) where T : UltimateDecision
        {
            return QueryDecisions<T>().FirstOrDefault(x => x.Protocol.RealityObject.Id == realityObjId);
        }

        public ICollection<T> GetActualDecisions<T>(IQueryable<RealityObject> realityObjIds) where T : UltimateDecision
        {
            if (realityObjIds == null || !realityObjIds.Any())
            {
                return new List<T>();
            }

            return QueryDecisions<T>()
                .WhereIf(realityObjIds.Any(), x => realityObjIds.Any(r => r == x.Protocol.RealityObject))
                .ToList();
        }

        private IQueryable<T> QueryDecisions<T>() where T: UltimateDecision
        {
            var domain = _container.Resolve<IDomainService<T>>();
            var protocolValidState =
                _container.Resolve<IDomainService<State>>()
                    .FirstOrDefault(x => x.TypeId == "gkh_real_obj_dec_protocol" && x.FinalState);

            var id = protocolValidState.Return(y => y.Id);

            return domain.GetAll()
                .Where(x => x.Protocol.DateStart <= DateTime.Today)
                .Where(x => x.Protocol.State.Id == id)
                .OrderByDescending(x => x.Protocol.ProtocolDate);
        }
    }
}