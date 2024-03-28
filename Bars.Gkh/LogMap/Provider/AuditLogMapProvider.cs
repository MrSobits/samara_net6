namespace Bars.Gkh.LogMap.Provider
{
    using Bars.B4.Modules.NHibernateChangeLog;

    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<BuilderLogMap>();
            container.Add<ContragentLogMap>();
            container.Add<EmergencyObjectLogMap>();
            container.Add<InspectorLogMap>();
            container.Add<ManagingOrganizationLogMap>();
            container.Add<MunicipalityLogMap>();
            container.Add<OperatorLogMap>();
            container.Add<PositionLogMap>();
            container.Add<RealityObjectLogMap>();
            container.Add<ServiceOrganizationLogMap>();
            container.Add<SupplyResourceOrgLogMap>();
            container.Add<WorkLogMap>();
            container.Add<RoomLogMap>();
            container.Add<ManOrgContractRealityObjectLogMap>();
            container.Add<RealityObjectImageLogMap>();
            container.Add<TehPassportValueLogMap>();
            container.Add<ContragentContactLogMap>();
        }
    }
}