namespace Bars.GkhGji.Regions.Saha.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class DisposalControlMeasuresLogMap : AuditLogMap<DisposalControlMeasures>
    {
        public DisposalControlMeasuresLogMap()
        {
            this.Name("Распоряжение - Мероприятия по контролю");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.ControlActivity.Code, "Code", "Код");
            this.MapProperty(x => x.ControlActivity.Name, "Name", "Наименование");
        }
    }
}