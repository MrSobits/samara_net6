namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class NsoActRemovalLogMap : AuditLogMap<NsoActRemoval>
    {
        public NsoActRemovalLogMap()
        {
            this.Name("Акт проверки предписания");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.AcquaintedWithDisposalCopy, "AcquaintedWithDisposalCopy", "С копией приказа ознакомлен");
            this.MapProperty(x => x.DocumentPlace, "DocumentPlace", "Место составления");
            this.MapProperty(x => x.DocumentTime, "DocumentTime", "Время составления акта;");
        }
    }
}