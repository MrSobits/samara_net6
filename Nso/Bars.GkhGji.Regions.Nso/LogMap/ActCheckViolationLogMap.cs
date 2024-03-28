namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.GkhGji.Entities;

    public class ActCheckViolationLogMap : AuditLogMap<ActCheckViolation>
    {
        public ActCheckViolationLogMap()
        {
            this.Name("Акт проверки - Нарушения");
            this.Description(x => x.Document.DocumentNumber ?? "");

            this.MapProperty(x => x.InspectionViolation.Violation.NormativeDocNames, "NormativeDocNames", "Пункты НПД");
            this.MapProperty(x => x.InspectionViolation.DatePlanRemoval, "DatePlanRemoval", "Срок устранения");
            this.MapProperty(x => x.InspectionViolation.DateFactRemoval, "DateFactRemoval", "Текст нарушения");
            this.MapProperty(x => x.ActObject.OfficialsGuiltyActions, "OfficialsGuiltyActions", "Сведения, свидетельствующие, что нарушения допущены в...");
            this.MapProperty(x => x.ActObject.PersonsWhoHaveViolated, "PersonsWhoHaveViolated", "Сведения о лицах, допустивших нарушения");
        }
    }
}