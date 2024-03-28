namespace Bars.GkhGji.Regions.Zabaykalye.Permissions
{
    using B4;
   
    public class GkhGjiPermissionMap: PermissionMap
    {
        public GkhGjiPermissionMap()
        {
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

            #region Акт обследования - поля
            Permission("GkhGji.DocumentsGji.ActSurvey.Field.DateOf_View", "Дата проведения - Просмотр");
            Permission("GkhGji.DocumentsGji.ActSurvey.Field.DateOf_Edit", "Дата проведения - Редактирование");

            Permission("GkhGji.DocumentsGji.ActSurvey.Field.TimeStart_View", "Время начала - Просмотр");
            Permission("GkhGji.DocumentsGji.ActSurvey.Field.TimeStart_Edit", "Время начала - Редактирование");

            Permission("GkhGji.DocumentsGji.ActSurvey.Field.TimeEnd_View", "Время окончания - Просмотр");
            Permission("GkhGji.DocumentsGji.ActSurvey.Field.TimeEnd_Edit", "Время окончания - Редактирование");

            Permission("GkhGji.DocumentsGji.ActSurvey.Field.ConclusionIssued_View", "Заключение вынесено - Просмотр");
            Permission("GkhGji.DocumentsGji.ActSurvey.Field.ConclusionIssued_Edit", "Заключение вынесено - Редактирование");
            #endregion

            #region Акт обследования - реестры
            Namespace("GkhGji.DocumentsGji.ActSurvey.Register.Conclusion", "Заключение о техническом состоянии");
            Permission("GkhGji.DocumentsGji.ActSurvey.Register.Conclusion.Edit", "Изменение записей");
            Permission("GkhGji.DocumentsGji.ActSurvey.Register.Conclusion.Create", "Создание записей");
            Permission("GkhGji.DocumentsGji.ActSurvey.Register.Conclusion.Delete", "Удаление записей");
            #endregion

            #region Отчеты
            Namespace("GkhGji.Report", "Отчеты");
            Permission("GkhGji.Report.AppealCitsJurnalAccounting", "Журнал учета обращений");
            Permission("GkhGji.Report.AdministrativeOffensesJurnalReport", "Журнал регистрации протоколов об административных правонарушениях");
            Permission("GkhGji.Report.ScheduledInspectionSurveysJournal", "Журнал учета плановых и внеплановых проверок, инспекционных обследований");
            Permission("GkhGji.Report.RegistrationOutgoingDocuments", "Регистрация исходящих документов");
            Permission("GkhGji.Report.AdministrativeOffensesResolution", "Журнал регистрации постановлений по делам об административных правонарушениях");
            Permission("GkhGji.Report.PrescriptionRegistrationJournal", "Журнал регистрации предписаний");
            #endregion Отчеты

            #region Справочники
            Namespace("GkhGji.Dict.LegalReason", "Правовые основания");
            CRUDandViewPermissions("GkhGji.Dict.LegalReason");

            Namespace("GkhGji.Dict.SurveySubject", "Предметы проверки");
            CRUDandViewPermissions("GkhGji.Dict.SurveySubject");
            #endregion

            Namespace("GkhGji.DocumentsGji.ActView", "Акты осмотра");
            Permission("GkhGji.DocumentsGji.ActView.View", "Просмотр");
        }
    }
}