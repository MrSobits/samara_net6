namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using GkhGji.Entities;

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