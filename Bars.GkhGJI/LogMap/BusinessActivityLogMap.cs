namespace Bars.GkhGji.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.GkhGji.Entities;
    using B4.Utils;

    public class BusinessActivityLogMap : AuditLogMap<BusinessActivity>
    {
        public BusinessActivityLogMap()
        {
            this.Name("Уведомления о начале предпринимательской деятельности");
            this.Description(x => x.Contragent.Name);

            this.MapProperty(x => x.Contragent, "Contragent", "Контрагент", x => x.Return(y => y.Name));
            this.MapProperty(x => x.Contragent, "Inn", "ИНН", x => x.Return(y => y.Inn));
        }
    }
}
