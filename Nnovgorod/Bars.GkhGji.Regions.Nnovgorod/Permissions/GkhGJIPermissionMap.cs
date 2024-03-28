namespace Bars.GkhGji.Regions.Nnovgorod.Permissions
{
    using B4;
    using GkhGji.Entities;
    
    public class GkhGjiPermissionMap: PermissionMap
    {
        public GkhGjiPermissionMap()
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

            #region Протокол - поля Реквизиты физического лица

            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonAddress_Edit", "Реквизиты физического лица: Адрес (место жительства, телефон)");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonJob_Edit", "Реквизиты физического лица: Место работы");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonPosition_Edit", "Реквизиты физического лица: Должность");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonBirthdayAndPlace_Edit", "Реквизиты физического лица: Дата, место рождения");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonDocument_Edit", "Реквизиты физического лица: Документ, удостоверяющий личность");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonSalary_Edit", "Реквизиты физического лица: Заработная плата");
            Permission("GkhGji.DocumentsGji.Protocol.Field.PhysPersonMaritalStatus_Edit", "Реквизиты физического лица: Семейное положение, кол-во иждивенцев");

            #endregion Протокол - поля Реквизиты физического лица

            #region Постановления  - поля

            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonAddress_Edit", "Реквизиты физического лица: Адрес (место жительства, телефон)");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonJob_Edit", "Реквизиты физического лица: Место работы");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonPosition_Edit", "Реквизиты физического лица: Должность");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonBirthdayAndPlace_Edit", "Реквизиты физического лица: Дата, место рождения");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonDocument_Edit", "Реквизиты физического лица: Документ, удостоверяющий личность");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonSalary_Edit", "Реквизиты физического лица: Заработная плата");
            Permission("GkhGji.DocumentsGji.Resolution.Field.PhysPersonMaritalStatus_Edit", "Реквизиты физического лица: Семейное положение, кол-во иждивенцев");

            Permission("GkhGji.DocumentsGji.Resolution.Field.BecameLegal_Edit", "Вступило в законную силу - Изменение");

            #endregion Постановления - поля

            #region Распоряжения - реестры

            Namespace("GkhGji.DocumentsGji.Disposal.Register.ControlMeasures", "Мероприятия по контролю");
            Permission("GkhGji.DocumentsGji.Disposal.Register.ControlMeasures.Create", "Создание записей");
            Permission("GkhGji.DocumentsGji.Disposal.Register.ControlMeasures.Edit", "Редактирование записей");
            Permission("GkhGji.DocumentsGji.Disposal.Register.ControlMeasures.Delete", "Удаление записей");

            #endregion Распоряжения - поля

            #region Отчеты
            Namespace("GkhGji.Report", "Отчеты");
            Permission("GkhGji.Report.AppealCitsJurnalAccounting", "Журнал учета обращений");
            Permission("GkhGji.Report.AdministrativeOffensesJurnalReport", "Журнал регистрации протоколов об административных правонарушениях");
            Permission("GkhGji.Report.ScheduledInspectionSurveysJournal", "Журнал учета плановых и внеплановых проверок, инспекционных обследований");
            Permission("GkhGji.Report.RegistrationOutgoingDocuments", "Регистрация исходящих документов");
            Permission("GkhGji.Report.AdministrativeOffensesResolution", "Журнал регистрации постановлений по делам об административных правонарушениях");
            Permission("GkhGji.Report.PrescriptionRegistrationJournal", "Журнал регистрации предписаний");
            #endregion Отчеты
        }
    }
}