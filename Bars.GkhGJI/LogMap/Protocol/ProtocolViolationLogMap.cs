namespace Bars.GkhGji.LogMap.Protocol
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class ProtocolViolationLogMap : AuditLogMap<ProtocolViolation>
    {
        public ProtocolViolationLogMap()
        {
            this.Name("Протакол - Нарушения");
            this.Description(x => x.Document.DocumentNumber ?? "");

            this.MapProperty(x => x.Description, "Action", "Мероприяти");
        }
    }
}