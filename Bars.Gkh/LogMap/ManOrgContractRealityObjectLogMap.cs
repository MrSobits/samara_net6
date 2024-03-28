namespace Bars.Gkh.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Entities;
    using B4.Utils;

    public class ManOrgContractRealityObjectLogMap : AuditLogMap<ManOrgContractRealityObject>
    {
        public ManOrgContractRealityObjectLogMap()
        {
            Name("Договор управления");

            Description(x => x.RealityObject.Return(y => y.Address));

            MapProperty(x => x.ManOrgContract.ManagingOrganization.Description, "Description", "Управляющая организация");
        }
    }

}
