namespace Bars.GkhGji.LogMap.Disposal
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class DisposalAdminRegulationLogMap : AuditLogMap<DisposalAdminRegulation>
    {
        public DisposalAdminRegulationLogMap()
        {
            this.Name("Распоряжение - Административные регламенты");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.AdminRegulation.Code, "Code", "Код");
            this.MapProperty(x => x.AdminRegulation.Name, "Name", "Эксперт");
        }
    }
}