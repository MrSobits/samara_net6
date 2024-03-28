namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class NsoDisposalLogMap : AuditLogMap<NsoDisposal>
    {
        public NsoDisposalLogMap()
        {
            this.Name("Рапоряжение ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentNumber, "DocumentNumber", "Номер документа");
            this.MapProperty(x => x.KindCheck.Name, "InspectionBaseType", "Вид Проверки");
            this.MapProperty(x => x.ObjectVisitStart, "ObjectVisitStart", "Период проведения проверки с");
            this.MapProperty(x => x.ObjectVisitEnd, "ObjectVisitEnd", "Период проведения проверки по");
            this.MapProperty(x => x.TimeVisitStart, "TimeVisitStart", "Время с");
            this.MapProperty(x => x.TimeVisitEnd, "TimeVisitEnd", "Время по");
            this.MapProperty(x => x.TypeAgreementProsecutor, "TypeAgreementProsecutor", "Согласование с прокуротурой", x => x.Return(y => y).GetDisplayName());
            this.MapProperty(x => x.TypeAgreementResult, "TypeAgreementResult", "Результат согласования", x => x.Return(y => y).GetDisplayName());
            this.MapProperty(x => x.IssuedDisposal.Fio, "IssuedDisposal.Fio", "ДЛ, вынесшее Распоряжение");
            this.MapProperty(x => x.ResponsibleExecution.Fio, "ResponsibleExecution.Fio", "Ответственный за исполнение");
            this.MapProperty(x => x.NcDate, "NcDate", "Дата документа (Уведомление о проверке)");
            this.MapProperty(x => x.NcNum, "NcNum", "Номер документа (Уведомление о проверке)");
            this.MapProperty(x => x.NcDateLatter, "NcDateLatter", "Дата исходящего пиьма  (Уведомление о проверке)");
            this.MapProperty(x => x.NcNumLatter, "NcNumLatter", "Номер исходящего письма  (Уведомление о проверке)");
            this.MapProperty(x => x.NcObtained, "NcObtained", "Уведомление получено (Уведомление о проверке)");
            this.MapProperty(x => x.NcSent, "NcSent", "Уведомление отправлено (Уведомление о проверке)");
            this.MapProperty(x => x.Inspection.InspectionBaseType.Name, "InspectionBaseType", "Основание проверки");
            this.MapProperty(x => x.Inspection.CheckDate, "CheckDate", "Дата проверки");
            this.MapProperty(x => x.Inspection.State.Name, "State", "Статус");
            this.MapProperty(x => x.TypeDocumentGji, "TypeDocumentGji", "Тип документа ГЖИ", x => x.Return(y => y).GetDisplayName());
            this.MapProperty(x => x.PoliticAuthority.ContragentName, "PoliticAuthority", "Орган гос власти");
            this.MapProperty(x => x.NoticeDescription, "NoticeDescription", "Время составления");
            this.MapProperty(x => x.PeriodCorrect, "PeriodCorrect", "Срок устранения");
            this.MapProperty(x => x.DateStatement, "DateStatement", "Дата формирования заявления");
            this.MapProperty(x => x.NoticeDateProtocol, "NoticeDateProtocol", "Дата составления протокола");
            this.MapProperty(x => x.ProsecutorDecDate, "ProsecutorDecDate", "Дата решения прокурора");
            this.MapProperty(x => x.ProsecutorDecNumber, "ProsecutorDecNumber", "Номер решения прокурораи");
            this.MapProperty(x => x.MotivatedRequestNumber, "MotivatedRequestNumber", "Номер мотивированного запроса");
            this.MapProperty(x => x.MotivatedRequestDate, "MotivatedRequestDate", "Номер решения прокурораи");
        }
    }
}