namespace Bars.Gkh.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Castle.Windsor;

    public class GkhRuleChangeStatusService : IGkhRuleChangeStatusService
    {
        public IWindsorContainer Container { get; set; }

        public List<IGkhRuleChangeStatus> GetGkhRuleStatus(string typeId, long newStateId, IStatefulEntity entity)
        {
            var stateTransferRuleDomain = Container.ResolveDomain<StateTransferRule>();
            var userRoleDomain = Container.ResolveDomain<UserRole>();
            var userIdentity = Container.Resolve<IUserIdentity>();
            var stateTransfersCache = Container.Resolve<IStateTransfersCache>();

            try
            {
                var result = new List<IGkhRuleChangeStatus>();

                var stateTransfers = stateTransfersCache.GetTransfers().Where(x => x.TypeId == typeId && x.NewState.Id == newStateId).ToArray();

                if (stateTransfers.Length > 0)
                {
                    var oldStateId = entity.State != null ? entity.State.Id : 0;

                    var roles = userRoleDomain.GetAll()
                        .Where(userRole => userRole.User.Id == userIdentity.UserId)
                        .Select(x => x.Role.Id)
                        .ToList();

                    if (roles.Any(x => stateTransfers.Where(y => y.CurrentState.Id == oldStateId && y.NewState.Id == newStateId)
                          .Select(y => y.Role.Id)
                          .Contains(x)))
                    {
                        var stateTransferRules = stateTransferRuleDomain.GetAll()
                            .Where(
                                x =>
                                    x.StateTransfer.CurrentState.Id == oldStateId &&
                                    x.StateTransfer.NewState.Id == newStateId)
                            .ToList()
                            .Where(x => roles.Contains(x.StateTransfer.Role.Id))
                            .Select(x => x.RuleId)
                            .ToList();

                        result = Container.ResolveAll<IRuleChangeStatus>()
                            .Where(x => stateTransferRules.Contains(x.Id))
                            .OfType<IGkhRuleChangeStatus>()
                            .ToList();
                    }
                }

                return result;
            }
            finally
            {
                Container.Release(stateTransferRuleDomain);
                Container.Release(userRoleDomain);
                Container.Release(userIdentity);
                Container.Release(stateTransfersCache);
            }
        }

        public IStatefulEntity GetEntity(string typeId, long entityId)
        {
            var sessionProvider = Container.Resolve<IStateProvider>();

            try
            {
                var statefulEntityInfo = sessionProvider.GetStatefulEntityInfo(typeId);

                var repositoryType = typeof(IDomainService<>).MakeGenericType(statefulEntityInfo.Type);

                var repository = Container.Resolve(repositoryType);

                var entityObject =
                    (IStatefulEntity)repositoryType.GetMethod("Get").Invoke(repository, new object[] { entityId });

                return entityObject;
            }
            finally
            {
                Container.Release(sessionProvider);
            }
        }
    }
}