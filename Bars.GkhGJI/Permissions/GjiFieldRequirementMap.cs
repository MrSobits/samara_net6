namespace Bars.GkhGji.Permissions
{
    using Bars.Gkh.DomainService;

    /// <summary>
    /// Обязательность полей
    /// </summary>
    public class GjiFieldRequirementMap : FieldRequirementMap
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public GjiFieldRequirementMap()
        {
            this.Namespace("GkhGji", "Модуль ГЖИ");
            this.Namespace("GkhGji.DocumentReestrGji", "Реестр документов ГЖИ");
            this.Namespace("GkhGji.DocumentReestrGji.ActCheck", "Акт проверки");
            this.Namespace("GkhGji.DocumentReestrGji.ActCheck.Field", "Поля");

            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.Area_Rqrd", "Площадь");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.Acquainted_With_The_Order", "С копией приказа ознакомлен");

            this.Namespace("GkhGji.AppealCits", "Реестр обращений ГЖИ");
            this.Namespace("GkhGji.AppealCits.Field", "Поля");
            this.Requirement("GkhGji.AppealCits.Field.Department_Rqrd", "Отдел");
            this.Namespace("GkhGji.AppealCits.Executant", "Исполнители");
            this.Namespace("GkhGji.AppealCits.Executant.Field", "Поля");

            this.Requirement("GkhGji.AppealCits.Executant.Field.PerformanceDate", "Срок исполнения");

            this.Namespace("GkhGji.PersonRegisterGji", "Реестр должностных лиц");
            this.Namespace("GkhGji.PersonRegisterGji.Field", "Поля");

            this.Requirement("GkhGji.PersonRegisterGji.Field.Email_Rqrd", "E-mail");
            this.Requirement("GkhGji.PersonRegisterGji.Field.Inn_Rqrd", "ИНН");
            this.Requirement("GkhGji.PersonRegisterGji.Field.TypeIdentityDocument_Rqrd", "Тип документа");
            this.Requirement("GkhGji.PersonRegisterGji.Field.IdIssuedDate_Rqrd", "Дата выдачи");
            this.Requirement("GkhGji.PersonRegisterGji.Field.IdSerial_Rqrd", "Серия");
            this.Requirement("GkhGji.PersonRegisterGji.Field.IdNumber_Rqrd", "Номер");
            this.Requirement("GkhGji.PersonRegisterGji.Field.IdIssuedBy_Rqrd", "Кем выдан");
            this.Requirement("GkhGji.PersonRegisterGji.Field.Contragent_Rqrd", "Управляющая организация");
            this.Requirement("GkhGji.PersonRegisterGji.Field.StartDate_Rqrd", "Дата начала");

            this.Namespace("GkhGji", "Модуль ГЖИ");
            this.Namespace("GkhGji.PersonRegisterGji", "Реестр должностных лиц");
            this.Namespace("GkhGji.PersonRegisterGji.Qualification", "Квалификационные аттестаты");
            this.Namespace("GkhGji.PersonRegisterGji.Qualification.Field", "Поля");

            this.Requirement("GkhGji.PersonRegisterGji.Qualification.Field.Number_Rqrd", "Номер КА");
            this.Requirement("GkhGji.PersonRegisterGji.Qualification.Field.BlankNumber_Rqrd", "Номер бланка");
            this.Requirement("GkhGji.PersonRegisterGji.Qualification.Field.IssuedDate_Rqrd", "Дата выдачи");
            this.Requirement("GkhGji.PersonRegisterGji.Qualification.Field.RecieveDate_Rqrd", "Дата получения");
            this.Requirement("GkhGji.PersonRegisterGji.Qualification.Field.FileIssueApplication_Rqrd", "Файл заявления о выдаче квалификационного аттестата");
            this.Requirement("GkhGji.PersonRegisterGji.Qualification.Field.ApplicationDate_Rqrd", "Дата подачи заявления о выдаче квалификационного аттестата");
            this.Requirement("GkhGji.PersonRegisterGji.Qualification.Field.FileNotificationOfExamResults_Rqrd", "Уведомление лицензионной комиссии о результатах экзамена");

            this.Namespace("GkhGji", "Модуль ГЖИ");
            this.Namespace("GkhGji.PersonRegisterGji", "Реестр должностных лиц");
            this.Namespace("GkhGji.PersonRegisterGji.RequestToExam", "Заявки на допуск к экзамену");
            this.Namespace("GkhGji.PersonRegisterGji.RequestToExam.Field", "Поля");

            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.RequestSupplyMethod_Rqrd", "Способ подачи заявления");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.RequestNum_Rqrd", "Номер заявки");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.RequestDate_Rqrd", "Дата заявки");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.RequestTime_Rqrd", "Время заявки");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.RequestFile_Rqrd", "Файл заявления");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.PersonalDataConsentFile_Rqrd", "Файл согласия на обработку перс.данных");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.NotificationNum_Rqrd", "Номер уведомления");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.NotificationDate_Rqrd", "Дата уведомления");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.IsDenied_Rqrd", "Отказ");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.ExamDate_Rqrd", "Дата экзамена");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.ExamTime_Rqrd", "Время экзамена");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.CorrectAnswersPercent_Rqrd", "Количество набранных баллов");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.ProtocolNum_Rqrd", "Номер протокола");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.ProtocolDate_Rqrd", "Дата протокола");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.ProtocolFile_Rqrd", "Файл протокола");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.ResultNotificationNum_Rqrd", "Номер уведомления (из блока 'Результаты экзамена')");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.ResultNotificationDate_Rqrd", "Дата уведомления (из блока 'Результаты экзамена')");
            this.Requirement("GkhGji.PersonRegisterGji.RequestToExam.Field.MailingDate_Rqrd", "Дата отправки почтой");

            this.Namespace("GkhGji", "Модуль ГЖИ");
            this.Namespace("GkhGji.ActivityTsj", "Деятельность ТСЖ");
            this.Namespace("GkhGji.ActivityTsj.Members", "Реестр членов ТСЖ/ЖСК");
            this.Namespace("GkhGji.ActivityTsj.Members.Field", "Поля");
            this.Requirement("GkhGji.ActivityTsj.Members.Field.File", "Файл реестра");

            this.Namespace("GkhGji", "Модуль ГЖИ");
            this.Namespace("GkhGji.Inspection", "Основания проверок");
            this.Namespace("GkhGji.Inspection.BaseDispHead", "Поручение руководителя");
            this.Namespace("GkhGji.Inspection.BaseDispHead.MainInfo", "Основная информация");
            this.Namespace("GkhGji.Inspection.BaseDispHead.MainInfo.Field", "Поля");

            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.DocumentName", "Наименование");
            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.DocumentNumber", "Номер");
            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.DocumentDate", "Дата");
            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.CheckDate", "Дата проверки");
            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.InnOfficial", "ИНН должностного лица");
            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.InnIndividual", "ИНН физ. лица");

            this.Namespace("GkhGji.Inspection.BaseJurPerson", "Плановые проверки юр.лиц");
            this.Namespace("GkhGji.Inspection.BaseJurPerson.Field", "Поля");
            this.Requirement("GkhGji.Inspection.BaseJurPerson.Field.UriRegistrationNumber", "Учетный номер проверки в едином реестре проверок");
            this.Requirement("GkhGji.Inspection.BaseJurPerson.Field.UriRegistrationDate", "Дата присвоения учетного номера");
            this.Requirement("GkhGji.Inspection.BaseJurPerson.Field.JurPersonInspectors", "Инспекторы");
            this.Requirement("GkhGji.Inspection.BaseJurPerson.Field.JurPersonZonalInspections", "Отделы");
            this.Requirement("GkhGji.Inspection.BaseJurPerson.Field.DateStart", "Дата проверки");
            
            this.Namespace("GkhGji.Inspection.ActionIsolated", "КНМ без взаимодействия с контролируемыми лицами");
            this.Namespace("GkhGji.Inspection.ActionIsolated.Field", "Поля");
            this.Requirement("GkhGji.Inspection.ActionIsolated.Field.CheckDate", "Дата начала проверки");
            
            this.Namespace("GkhGji.Dicts", "Справочники");
            this.Namespace("GkhGji.Dicts.Inspector", "Инспекторы");
            this.Namespace("GkhGji.Dicts.Inspector.Field", "Поля");
            this.Requirement("GkhGji.Dicts.Inspector.Fields.InspectorPosition", "Должность");
        }
    }
}
