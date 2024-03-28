namespace Bars.GkhGji.LogMap.ActCheck
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ActCheckDefinitionLogMap : AuditLogMap<ActCheckDefinition>
    {
        public ActCheckDefinitionLogMap()
        {
            this.Name("Акт проверки - Определения");
            this.Description(x => x.ActCheck.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер документа");
            this.MapProperty(x => x.ExecutionDate, "ExecutionDate", "Дата исполнения");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.IssuedDefinition.Fio, "IssuedDefinition", "ДЛ, вынесшее определение");
            this.MapProperty(x => x.TypeDefinition, "TypeDefinition", "Тип определения", x => x.Return(y => y.GetDisplayName()));
        }
    }
}