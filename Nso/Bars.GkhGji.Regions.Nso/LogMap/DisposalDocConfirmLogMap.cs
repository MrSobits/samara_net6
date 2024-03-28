namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Entities.Disposal;

    public class DisposalDocConfirmLogMap : AuditLogMap<DisposalDocConfirm>
    {
        public DisposalDocConfirmLogMap()
        {
            this.Name("Распоряжение - Документы на согласование");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentName, "DocumentName", "Наименование");
        }
    }
}