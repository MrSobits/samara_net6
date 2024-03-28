namespace Bars.Gkh.RegOperator.LogMap
{
    using System;
    using System.Linq;
    using B4;
    using B4.Application;
    using B4.IoC;
    using B4.Modules.NHibernateChangeLog;

    using Bars.B4.DataAccess;

    using Decisions.Nso.Entities;
    using Gkh.Entities;

    using NHibernate;

    public abstract class UltimateDecisionLogMap<T> : AuditLogMap<T>
        where T : UltimateDecision
    {
        protected UltimateDecisionLogMap()
        {
            this.Description(x =>
            {
                var container = ApplicationContext.Current.Container;
                var sessionProvider = container.Resolve<ISessionProvider>();

                var result = x.Protocol.RealityObject.Address;
                if (result == null)
                {
                    using (container.Using(sessionProvider))
                    using (var statelessSession = sessionProvider.OpenStatelessSession())
                    {
                        try
                        {
                            const string query = "SELECT ADDRESS FROM GKH_REALITY_OBJECT where id = :id";

                            result = statelessSession
                                .CreateSQLQuery(query)
                                .SetParameter("id", x.Protocol.RealityObject.Id)
                                .List<string>()
                                .FirstOrDefault();
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }

                return result ?? string.Empty;
            });
        }
    }
}
