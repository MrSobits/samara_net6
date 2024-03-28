namespace Bars.Gkh.Decisions.Nso.LogMap
{
    using System.Linq;
    using B4;
    using B4.Application;
    using B4.IoC;
    using B4.Modules.NHibernateChangeLog;
    using Entities;
    using Gkh.Entities;

    public abstract class UltimateDecisionLogMap<T> : AuditLogMap<T>
        where T : UltimateDecision
    {
        protected UltimateDecisionLogMap()
        {
            this.Description(x =>
            {
                string result = x.Protocol.RealityObject.Address;
                if (result == null)
                {
                    var container = ApplicationContext.Current.Container;
                    var roService = container.Resolve<IDomainService<RealityObject>>();
                    using (container.Using(roService))
                    {
                        result = roService.GetAll()
                            .Where(r => r.Id == x.Protocol.RealityObject.Id)
                            .Select(r => r.Address)
                            .FirstOrDefault();
                    }
                }

                return result ?? string.Empty;
            });
        }
    }
}
