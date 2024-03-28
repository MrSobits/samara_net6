namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.ActRemoval;

    public class ActRemovalInspectedPartLogMap : AuditLogMap<ActRemovalInspectedPart>
    {
        public ActRemovalInspectedPartLogMap()
        {
            this.Name("Акт проверки предписания - Инспектируемые части");
            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.ActRemoval.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.ActRemoval.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.InspectedPart.Name, "InspectedPart", "Инспектируемая часть");
            this.MapProperty(x => x.Character, "Character", "Характер и местоположение");
            this.MapProperty(x => x.Description, "Description", "Описание");
        }
    }
}