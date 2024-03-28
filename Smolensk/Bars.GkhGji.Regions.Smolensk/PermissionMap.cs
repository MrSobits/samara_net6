namespace Bars.GkhGji.Regions.Smolensk
{
    using Bars.GkhGji.Entities;

    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {

            Namespace<AppealCits>("GkhGji.AppealCitizensState.Field.AppealNumber", "Номер обращения");
            Permission("GkhGji.AppealCitizensState.Field.AppealNumber.Edit", "Изменение");

            Namespace<AppealCits>("GkhGji.AppealCitizensState.Field.DateFrom", "От");
            Permission("GkhGji.AppealCitizensState.Field.DateFrom.Edit", "Изменение");

            Namespace<AppealCits>("GkhGji.AppealCitizensState.Field.Year", "Год");
            Permission("GkhGji.AppealCitizensState.Field.Year.Edit", "Изменение");

            Namespace("GkhGji.AppealCitizens.Field", "Поля");
            Namespace<AppealCits>("GkhGji.AppealCitizens.Field.ArchiveNumber", "Архивный номер");
            Permission("GkhGji.AppealCitizens.Field.ArchiveNumber.View", "Просмотр");

            Permission("GkhGji.DocumentsGji.ActCheck.Field.ActToPres_Edit", "Акт направлен в прокуратуру - Изменение");

            // Протокол - поля Реквизиты физического лица
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonAddress_Edit", "Реквизиты физического лица: Адрес (место жительства, телефон)");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonJob_Edit", "Реквизиты физического лица: Место работы");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonPosition_Edit", "Реквизиты физического лица: Должность");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonBirthdayAndPlace_Edit", "Реквизиты физического лица: Дата, место рождения");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonDocument_Edit", "Реквизиты физического лица: Документ, удостоверяющий личность");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonSalary_Edit", "Реквизиты физического лица: Заработная плата");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonMaritalStatus_Edit", "Реквизиты физического лица: Семейное положение, кол-во иждивенцев");
            Permission("GkhGji.DocumentsGji.Protocol.Field.NoticeDocNumber_Edit", "Уведомление номер дкоумента - Редактирование");
            Permission("GkhGji.DocumentsGji.Protocol.Field.NoticeDocDate_Edit", "Уведомление дата дкоумента - Редактирование");


            // Протокол - реестры
            Permission("GkhGji.DocumentsGji.Protocol.Register.Definition.DescriptionSet", "Установил");
            Permission("GkhGji.DocumentsGji.Protocol.Register.Definition.DefinitionResult", "Результат определения");
            Permission("GkhGji.DocumentsGji.Protocol.Register.Definition.ExtendUntil", "Продлить до");

            // Постановления  - поля
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonAddress_Edit", "Реквизиты физического лица: Адрес (место жительства, телефон)");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonJob_Edit", "Реквизиты физического лица: Место работы");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonPosition_Edit", "Реквизиты физического лица: Должность");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonBirthdayAndPlace_Edit", "Реквизиты физического лица: Дата, место рождения");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonDocument_Edit", "Реквизиты физического лица: Документ, удостоверяющий личность");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonSalary_Edit", "Реквизиты физического лица: Заработная плата");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonMaritalStatus_Edit", "Реквизиты физического лица: Семейное положение, кол-во иждивенцев");
            Permission("GkhGji.DocumentsGji.Resolution.Field.BecameLegal_Edit", "Вступило в законную силу - Изменение");

            // Постановление - реестры
            Permission("GkhGji.DocumentsGji.Resolution.Register.Definition.DescriptionSet", "Установил");
            Permission("GkhGji.DocumentsGji.Resolution.Register.Definition.DefinitionResult", "Результат определения");

            // распоряжение - поля
            Permission("GkhGji.DocumentsGji.Disposal.Field.VerifPurpose_View", "Цель проверки - Просмотр");
            Permission("GkhGji.DocumentsGji.Disposal.Field.VerifPurpose_Edit", "Цель проверки - Редактирование");

            // Распоряжения - реестры
            Namespace("GkhGji.DocumentsGji.Disposal.Register.ControlMeasures", "Мероприятия по контролю");
            Permission("GkhGji.DocumentsGji.Disposal.Register.ControlMeasures.Create", "Создание записей");
            Permission("GkhGji.DocumentsGji.Disposal.Register.ControlMeasures.Edit", "Редактирование записей");
            Permission("GkhGji.DocumentsGji.Disposal.Register.ControlMeasures.Delete", "Удаление записей");

            // Предписание - реестры
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.DocumentDate", "Дата");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.DocumentNum", "Номер документа");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.DateCancel", "Дата отмены");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.IssuedCancel", "ДЛ, вынесшее решение");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.IsCourt", "Отменено судом");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.Reason", "Причина отмены");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.PetitionNum", "Номер ходатайства");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.PetitionDate", "Дата ходатайства");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.DescriptionSet", "Установлено");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.CancelResult", "Результат решения");
            Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.TypeCancel", "Тип решения");

            // Отчеты
            Namespace("GkhGji.Report", "Отчеты");
            Permission("GkhGji.Report.AppealCitsJurnalAccounting", "Журнал учета обращений");
            Permission("GkhGji.Report.AdministrativeOffensesJurnalReport", "Журнал регистрации протоколов об административных правонарушениях");
            Permission("GkhGji.Report.ScheduledInspectionSurveysJournal", "Журнал учета плановых и внеплановых проверок, инспекционных обследований");
            Permission("GkhGji.Report.RegistrationOutgoingDocuments", "Регистрация исходящих документов");
            Permission("GkhGji.Report.AdministrativeOffensesResolution", "Журнал регистрации постановлений по делам об административных правонарушениях");
            Permission("GkhGji.Report.PrescriptionRegistrationJournal", "Журнал регистрации предписаний");

            Namespace("GkhGji.Dict.SurveySubject", "Предметы проверки");
            CRUDandViewPermissions("GkhGji.Dict.SurveySubject");
        }
    }
}