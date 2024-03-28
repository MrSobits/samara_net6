namespace Bars.Gkh.Overhaul.Tat.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class DpkrCorrectionLogMap : AuditLogMap<DpkrCorrectionStage2>
    {
        public DpkrCorrectionLogMap()
        {
            Name("Результат корректировки");

            Description(v => v.Stage2.CommonEstateObject.Name);

            MapProperty(v => v.PlanYear, "PlanYear", "Скорректированный год");
        }
    }
}
