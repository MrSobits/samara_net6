namespace Bars.GkhGji.LogMap.ActCheck
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;

    public class ActCheckLogMap : AuditLogMap<Entities.ActCheck>
    {
        public ActCheckLogMap()
        {
            this.Name("Документы ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.TypeDocumentGji, "TypeDocumentGji", "Тип документа");
            this.MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.Inspection.InspectionBaseType.Name, "InspectionBaseType", "Основание проверки", x => x.Return(y => y));
            this.MapProperty(x => x.Inspection.CheckDate, "CheckDate", "Дата проверки", x => x.Return(y => y.ToString()));
            this.MapProperty(x => x.Inspection.State.Name, "State", "Статус", x => x.Return(y => y));
            this.MapProperty(x => x.TypeDocumentGji, "TypeDocumentGji", "Тип документа ГЖИ", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.Inspection.InspectionBaseType.Name, "InspectionBaseType", "Основание проверки", x => x.Return(y => y));
            this.MapProperty(x => x.DocumentPlace, "DocumentPlace", "Место составления", x => x.Return(y => y));
            this.MapProperty(x => x.DocumentTime, "DocumentTime", "Время составления Акта", x => x.Return(y => y).ToString());
            this.MapProperty(x => x.AcquaintState, "DocumentPlace", "Статус ознакомления с результатами проверки", x => x.Return(y => y).ToString());
        }
    }
}
