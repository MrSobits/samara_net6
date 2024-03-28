namespace Bars.GkhGji.LogMap
{
    using B4.Utils;
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class ActivityTsjLogMap : AuditLogMap<ActivityTsj>
    {
        public ActivityTsjLogMap()
        {
            this.Name("Деятельность ТСЖ");
            this.Description(x => x.ReturnSafe(y => y.ManagingOrganization.Contragent.Name));
            this.MapProperty(x => x.ManagingOrganization.Contragent, "Tsj", "ТСЖ", x => x.Return(y => y.Name));
        }
    }
}
