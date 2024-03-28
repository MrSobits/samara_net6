namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.ActRemoval;

    public class ChelyabinskActRemovalLogMap : AuditLogMap<ChelyabinskActRemoval>
    {
        public ChelyabinskActRemovalLogMap()
        {
            this.Name("Акт проверки предписания");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.AcquaintedWithDisposalCopy, "PhysicalPersonInfo", "С копией приказа ознакомлен");
            this.MapProperty(x => x.DocumentPlace, "PhysicalPersonInfo", "Место составления");
            this.MapProperty(x => x.DocumentTime, "PhysicalPersonInfo", "Время составления акта;");
        }
    }
}