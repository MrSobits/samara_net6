namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class NsoProtocolLogMap : AuditLogMap<NsoProtocol>
    {
        public NsoProtocolLogMap()
        {
            this.Name("Протокол ГЖИ");
            this.Description(x => x.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.PersonPosition, "PersonPosition", "Должность");
            this.MapProperty(x => x.PersonBirthDatePlace, "PersonBirthDatePlace", "Дата, место рождения");
            this.MapProperty(x => x.PersonRegistrationAddress, "PersonRegistrationAddress", " Адрес регистрации (место жительства, телефон)");
            this.MapProperty(x => x.PersonFactAddress, "PersonFactAddress", "Фактический адрес");
            this.MapProperty(x => x.TypePresence, "TypePresence", "Протокол - Реквизиты - В присуствии/отсутствии", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.Representative, "Representative", "Представитель");
            this.MapProperty(x => x.ReasonTypeRequisites, "ReasonTypeRequisites", "Вид и реквизиты основания");
            this.MapProperty(x => x.ResolveViolationClaim.Name, "ResolveViolationClaim", "Наименование требования");
            this.MapProperty(x => x.DateOfViolation, "DateOfViolation", "Нарушения - Дата правонарушения");
            this.MapProperty(x => x.HourOfViolation, "HourOfViolation", "Нарушения - Час правонарушения");
            this.MapProperty(x => x.MinuteOfViolation, "MinuteOfViolation", "Нарушения - Минута правонарушения");
            this.MapProperty(x => x.Contragent.Name, "Contragent", "Контрагент");
            this.MapProperty(x => x.PhysicalPersonInfo, "PhysicalPersonInfo", "Реквизиты физ. лица");
            this.MapProperty(x => x.Executant.Name, "Executant", "Тип исполнителя документа");
            this.MapProperty(x => x.TypeDocumentGji, "TypeDocumentGji", "Тип документа ГЖИ", x => x.Return(y => y.GetDisplayName()));
            this.MapProperty(x => x.Inspection.InspectionBaseType.Name, "InspectionBaseType", "Основание проверки");
            this.MapProperty(x => x.NotifDeliveredThroughOffice, "NotifDeliveredThroughOffice", "Доставлено через канцелярию");
            this.MapProperty(x => x.FormatDate, "FormatDate", "Дата вручения (регистрации) уведомления");
            this.MapProperty(x => x.NotifNumber, "NotifNumber", "Номер регистрации");
            this.MapProperty(x => x.DateOfProceedings, "DateOfProceedings", "Дата рассмотрения дела");
            this.MapProperty(x => x.HourOfProceedings, "HourOfProceedings", "Время рассмотрения дела(час)");
            this.MapProperty(x => x.MinuteOfProceedings, "MinuteOfProceedings", "Время рассмотрения дела(мин)");
            this.MapProperty(x => x.ProceedingCopyNum, "ProceedingCopyNum", "Количество экземпляров");
            this.MapProperty(x => x.ProceedingsPlace, "ProceedingsPlace", "Место рассмотрения дела");
            this.MapProperty(x => x.Remarks, "Remarks", "Замечания к протоколу со стороны нарушителя");
        }
    }
}