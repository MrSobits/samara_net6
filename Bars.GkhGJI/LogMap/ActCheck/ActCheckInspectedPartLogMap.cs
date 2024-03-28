namespace Bars.GkhGji.LogMap.ActCheck
{
    using B4.Modules.NHibernateChangeLog;

    using Bars.B4.Utils;

    using Entities;

    public class ActCheckInspectedPartLogMap : AuditLogMap<ActCheckInspectedPart>
    {
        public ActCheckInspectedPartLogMap()
        {
            this.Name("Акт проверки - Инспектируемые части");
            this.Description(x => x.ActCheck.DocumentNumber ?? "");

            this.MapProperty(x => x.Character, "Character", "Характер и местоположение");
            this.MapProperty(x => x.Description, "Description", "Примечание");
        }
    }
}