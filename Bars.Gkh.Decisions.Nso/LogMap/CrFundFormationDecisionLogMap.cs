namespace Bars.Gkh.Decisions.Nso.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;

    public class CrFundFormationDecisionLogMap : AuditLogMap<CrFundFormationDecision>
    {
            public CrFundFormationDecisionLogMap()
        {
            Name("Решение о формировании фонда Кр");

            Description(x => x.ReturnSafe(y => y.Protocol.RealityObject.Address));

            MapProperty(x => x.Decision, "Decision", "Решение");
        }
    }
}
