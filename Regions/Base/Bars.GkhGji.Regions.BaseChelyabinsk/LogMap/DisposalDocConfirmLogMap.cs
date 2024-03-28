namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.Disposal;

    public class DisposalDocConfirmLogMap : AuditLogMap<DisposalDocConfirm>
    {
        public DisposalDocConfirmLogMap()
        {
            this.Name("Распоряжение - Документы на согласование");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.Disposal.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Disposal.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.DocumentName, "DocumentName", "Наименование");
        }
    }
}
