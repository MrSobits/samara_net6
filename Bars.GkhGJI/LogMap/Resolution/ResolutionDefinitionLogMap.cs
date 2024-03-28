namespace Bars.GkhGji.LogMap.Resolution
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ResolutionDefinitionLogMap : AuditLogMap<ResolutionDefinition>
    {
        public ResolutionDefinitionLogMap()
        {
            this.Name("Постановление - Определения");
            this.Description(x => x.Resolution.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер документа");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.TypeDefinition, "TypeDefinition", "Тип определения", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.IssuedDefinition.Fio, "IssuedDefinition", "ДЛ, вынесшее определение");
            this.MapProperty(x => x.ExecutionDate, "ExecutionDate", "Дата исполнения");
            this.MapProperty(x => x.Description, "TypeDefinition", "Тип определения");
        }
    }
}