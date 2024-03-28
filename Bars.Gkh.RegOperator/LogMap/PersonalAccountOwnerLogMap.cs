namespace Bars.Gkh.RegOperator.LogMap
{
    using Bars.Gkh.RegOperator.Entities;
    using Bars.B4.Modules.NHibernateChangeLog;

    public class PersonalAccountOwnerLogMap : AuditLogMap<PersonalAccountOwner>
    {
        public PersonalAccountOwnerLogMap()
        {
            Name("Абонент");

            Description(x => x.Name);
            MapProperty(x => x.Name, "Name", "ФИО Абонента");
        }
    }
}
