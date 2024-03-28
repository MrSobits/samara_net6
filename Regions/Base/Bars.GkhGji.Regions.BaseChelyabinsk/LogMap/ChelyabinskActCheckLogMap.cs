namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;

    using Bars.B4.Utils;

    using Entities.ActCheck;

    public class ChelyabinskActCheckLogMap : AuditLogMap<ChelyabinskActCheck>
    {
        public ChelyabinskActCheckLogMap()
        {
            this.Name("Акт проверки ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.TypeDocumentGji, "TypeDocumentGji", "Тип документа");
            this.MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер документа");
            this.MapProperty(x => x.Inspection.InspectionBaseType.Name, "InspectionBaseType", "Основание проверки");
            this.MapProperty(x => x.Inspection.CheckDate, "CheckDate", "Дата проверки");
            this.MapProperty(x => x.TypeDocumentGji, "TypeDocumentGji", "Тип документа ГЖИ", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.Inspection.InspectionBaseType.Name, "InspectionBaseType", "Основание проверки");
            this.MapProperty(x => x.DocumentPlace, "DocumentPlace", "Место составления");
            this.MapProperty(x => x.DocumentTime, "DocumentTime", "Время составления Акта", x => x.Return(y => y).ToString());
            this.MapProperty(x => x.AcquaintState, "DocumentPlace", "Статус ознакомления с результатами проверки", x => x.Return(y => y).ToString());
            this.MapProperty(x => x.AcquaintedWithDisposalCopy, "AcquaintedWithDisposalCopy", "С копией приказа ознакомлен");
        }
    }
}
