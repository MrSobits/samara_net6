namespace Bars.GkhGji.LogMap.Disposal
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class DisposalExpertLogMap : AuditLogMap<DisposalExpert>
    {
        public DisposalExpertLogMap()
        {
            this.Name("Распоряжение - Эксперты");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.Expert.Name, "Name", "Эксперт");
        }
    }
}