namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class ActRemovalInspectedPartLogMap : AuditLogMap<ActRemovalInspectedPart>
    {
        public ActRemovalInspectedPartLogMap()
        {
            this.Name("Акт проверки предписания - Инспектируемые части");

            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.InspectedPart.Name, "InspectedPart", "Инспектируемая часть ГЖИ");
            this.MapProperty(x => x.Character, "Character", "Характер и местоположение");
            this.MapProperty(x => x.Description, "Description", "Описание");
        }
    }
}