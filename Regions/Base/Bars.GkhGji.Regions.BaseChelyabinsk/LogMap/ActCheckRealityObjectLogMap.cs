namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.GkhGji.Entities;

    public class ActCheckRealityObjectLogMap : AuditLogMap<ActCheckRealityObject>
    {
        public ActCheckRealityObjectLogMap()
        {
            this.Name("Акт проверки - Нарушения");
            this.Description(x => x.ActCheck.DocumentNumber ?? "");

            this.MapProperty(x => x.PersonsWhoHaveViolated, "PersonsWhoHaveViolated", "Сведения о лицах, допустивших нарушения");
            this.MapProperty(x => x.OfficialsGuiltyActions, "OfficialsGuiltyActions", "Сведения, свидетельствующие, что нарушения допущены в...");
        }
    }
}