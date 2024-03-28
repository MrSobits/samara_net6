namespace Bars.Gkh.RegOperator.LogMap
{
    using Bars.Gkh.RegOperator.Entities;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;

    public class PersonalAccountPeriodSummaryLogMap : AuditLogMap<PersonalAccountPeriodSummary>
    {
        public PersonalAccountPeriodSummaryLogMap()
        {
            Name("Состояние лицевого счета за период");

            Description(x => x.ReturnSafe(y => string.Format("{0} - {1}", y.PersonalAccount.PersonalAccountNum, y.Period.Name)));

            MapProperty(x => x.SaldoOut, "SaldoOut", "Исходящее сальдо");
        }
    }
}
