namespace Bars.GkhGji.LogMap.Resolution
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ResolutionPayFineLogMap : AuditLogMap<ResolutionPayFine>
    {
        public ResolutionPayFineLogMap()
        {
            this.Name("Постановление - Оплата штрафов");
            this.Description(x => x.Resolution.DocumentNumber ?? "");

            this.MapProperty(x => x.TypeDocumentPaid, "TypeDocumentPaid", "Номер документа", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.DocumentNum, "DocumentNum", "Дата документа");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Amount, "Amount", "Дата документа");
        }
    }
}