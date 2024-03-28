namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.ActCheck;

    public class ActCheckRoLongDescriptionLogMap : AuditLogMap<ActCheckRoLongDescription>
    {
        public ActCheckRoLongDescriptionLogMap()
        {
            this.Name("Акт проверки - Результаты проверки");
            this.Description(x => x.ActCheckRo.ActCheck.DocumentNumber ?? "");

            this.MapProperty(x => x.ActCheckRo.NotRevealedViolations, "NotRevealedViolations", "Не выявленные нарушения");
            this.MapProperty(x => x.ActCheckRo.HaveViolation, "HaveViolation", "Признак выявлено или невыявлено нарушение");
        }
    }
}
