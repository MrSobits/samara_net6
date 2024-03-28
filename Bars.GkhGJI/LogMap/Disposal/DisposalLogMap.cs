namespace Bars.GkhGji.LogMap.Disposal
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;

    public class DisposalLogMap : AuditLogMap<Entities.Disposal>
    {
        public DisposalLogMap()
        {
            this.Name("Рапоряжение ГЖИ");

            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));

            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.KindCheck.Name, "InspectionBaseType", "Вид Проверки");
            this.MapProperty(x => x.ObjectVisitStart, "Период проведения проверки с", "с");
            this.MapProperty(x => x.ObjectVisitEnd, "Период проведения проверки по", "по");
            this.MapProperty(x => x.TimeVisitStart, "TimeVisitStart", "Время с");
            this.MapProperty(x => x.TimeVisitEnd, "Время по", "по", x => x.Return(y => y).ToString());
            this.MapProperty(x => x.TypeAgreementProsecutor, "TypeAgreementProsecutor", "Согласование с прокуротурой", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.TypeAgreementResult, "TypeAgreementResult", "Результат согласования", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.IssuedDisposal.Fio, "IssuedDisposal.Fio", "ДЛ, вынесшее Распоряжение");
            this.MapProperty(x => x.ResponsibleExecution.Fio, "ResponsibleExecution.Fio", "Ответственный за исполнение");
            this.MapProperty(x => x.NcDate, "NcDate", "Дата документа (Уведомление о проверке)");
            this.MapProperty(x => x.NcNum, "NcNum", "Номер документа (Уведомление о проверке)");
            this.MapProperty(x => x.NcDateLatter, "NcDateLatter", "Дата исходящего пиьма  (Уведомление о проверке)");
            this.MapProperty(x => x.NcNumLatter, "NcNumLatter", "Номер исходящего письма  (Уведомление о проверке)");
            this.MapProperty(x => x.NcObtained, "NcObtained", "Уведомление получено (Уведомление о проверке)");
            this.MapProperty(x => x.NcSent, "NcSent", "Уведомление отправлено (Уведомление о проверке)");
            this.MapProperty(x => x.Inspection.InspectionBaseType.Name, "InspectionBaseType", "Основание проверки", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.Inspection.CheckDate, "CheckDate", "Дата проверки" );
            this.MapProperty(x => x.Inspection.State, "State", "Статус");
        }
    }
}
