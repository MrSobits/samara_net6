namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckProvidedDocLogMap : AuditLogMap<ActCheckProvidedDoc>
    {
        public ActCheckProvidedDocLogMap()
        {
            this.Name("Акт проверки - Предоставленные документы");
            this.Description(x => x.ActCheck.DocumentNumber ?? "");
            this.MapProperty(x => x.DateProvided, "DateProvided", "Дата предосталвения", x => x.Return(y => y.ToString()));
        }
    }
}