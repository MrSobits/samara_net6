namespace Bars.GkhGji.LogMap.Protocol
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ProtocolDefinitionLogMap : AuditLogMap<ProtocolDefinition>
    {
        public ProtocolDefinitionLogMap()
        {
            this.Name("Протокол - Определения");
            this.Description(x => x.Protocol.DocumentNumber ?? "");

            this.MapProperty(x => x.ExecutionDate, "ExecutionDate", "Дата исполнения");
            this.MapProperty(x => x.IssuedDefinition.Fio, "IssuedDefinition", "ДЛ, вынесшее определение");
            this.MapProperty(x => x.TypeDefinition, "TypeDefinition", "Тип определения", x => x.Return(y => y.GetDisplayName()));
        }
    }
}