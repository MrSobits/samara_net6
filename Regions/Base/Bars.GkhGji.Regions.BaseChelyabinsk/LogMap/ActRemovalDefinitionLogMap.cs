namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.ActRemoval;

    public class ActRemovalDefinitionLogMap : AuditLogMap<ActRemovalDefinition>
    {
        public ActRemovalDefinitionLogMap()
        {
            this.Name("Акт проверки предписания - Определения");
            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.ActRemoval.DocumentDate, "ActRemovalDocumentDate", "Дата документа");
            this.MapProperty(x => x.ActRemoval.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.DocumentNum, "DocumentNum", "Номер документа");
            this.MapProperty(x => x.ExecutionDate, "ExecutionDate", "Дата исполнения");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.IssuedDefinition.Fio, "IssuedDefinition", "ФИО");
            this.MapProperty(x => x.TypeDefinition, "TypeDefinition", "Тип определения", x => x.Return(y => y.GetDisplayName()));
        }
    }
}