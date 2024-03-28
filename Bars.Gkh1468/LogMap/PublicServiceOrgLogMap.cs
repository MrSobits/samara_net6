namespace Bars.Gkh1468.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    public class PublicServiceOrgLogMap : AuditLogMap<PublicServiceOrg>
    {
        public PublicServiceOrgLogMap()
        {
            Name("Поставщики ресурсов");

            Description(x => x.Contragent.Return(y => y.Name ?? string.Empty));

            MapProperty(x => x.Contragent, "Contragent", "Контрагент", x => x.Return(y => y.Name));
            MapProperty(x => x.Contragent, "Inn", "ИНН", x => x.Return(y => y.Inn));
        }
    }
}
