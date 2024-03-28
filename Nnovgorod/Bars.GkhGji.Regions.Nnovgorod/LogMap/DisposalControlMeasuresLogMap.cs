namespace Bars.GkhGji.Regions.Nnovgorod.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class DisposalControlMeasuresLogMap : AuditLogMap<DisposalControlMeasures>
    {
        public DisposalControlMeasuresLogMap()
        {
            this.Name("Распоряжение - Мероприятия по контролю");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.ControlMeasuresName, "Name", "Наименование");
        }
    }
}