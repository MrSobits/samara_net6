namespace Bars.Gkh.Decisions.Nso.LogMap.Provider
{
    using Bars.B4.Modules.NHibernateChangeLog;

    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            // логирование этих сущностей падает с ошибкой
            //container.Add<CrFundFormationDecisionLogMap>();
            //container.Add<MonthlyFeeAmountDecisionLogMap>();
        }
    }
}