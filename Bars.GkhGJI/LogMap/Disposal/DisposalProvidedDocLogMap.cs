namespace Bars.GkhGji.LogMap.Disposal
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class DisposalProvidedDocLogMap : AuditLogMap<DisposalProvidedDoc>
    {
        public DisposalProvidedDocLogMap()
        {
            this.Name("Распоряжение - Предоставляемые документы");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.ProvidedDoc.Code, "Code", "Код");
            this.MapProperty(x => x.ProvidedDoc.Name, "Name", "Наименование");
        }
    }
}