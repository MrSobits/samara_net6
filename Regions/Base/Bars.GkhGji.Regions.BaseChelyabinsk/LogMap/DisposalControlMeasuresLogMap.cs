namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities.Disposal;

    public class DisposalControlMeasuresLogMap : AuditLogMap<DisposalControlMeasures>
    {
        public DisposalControlMeasuresLogMap()
        {
            this.Name("Распоряжение - Мероприятия по контролю");
            this.Description(x => x.Disposal.DocumentNumber ?? "");

            this.MapProperty(x => x.Disposal.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.Disposal.DocumentNumber, "DocumentNumber", "Номер документа");
            this.MapProperty(x => x.ControlActivity.Code, "Code", "Код");
            this.MapProperty(x => x.ControlActivity.Name, "Name", "Наименование");
        }
    }
}
