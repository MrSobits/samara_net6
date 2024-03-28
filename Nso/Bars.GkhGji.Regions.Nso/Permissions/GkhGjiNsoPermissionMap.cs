namespace Bars.GkhGji.Regions.Nso.Permissions
{
    using B4;
    using Entities;
    using Bars.GkhGji.Entities;

    public class GkhGjiNsoPermissionMap : PermissionMap
    {
        public GkhGjiNsoPermissionMap()
        {
            #region Управление задачами
            this.Namespace("GkhGji.ManagementTask.ReminderAppealCits", "Задачи по обращениям");
            this.Permission("GkhGji.ManagementTask.ReminderAppealCits.View", "Просмотр");
            #endregion

            #region Справочники
            this.Permission("GkhGji.AppealCitizensState.Field.Consideration.ApprovalContragent_View", "Контрагент");

            this.Namespace<MkdChangeNotification>("GkhGji.MkdChangeNotification", "Реестр уведомлений о смене способа управления МКД");
            this.CRUDandViewPermissions("GkhGji.MkdChangeNotification");

            this.Namespace("GkhGji.MkdChangeNotification.Fields", "Поля");
            this.Permission("GkhGji.MkdChangeNotification.Fields.Date.Edit", "Дата");
            this.Permission("GkhGji.MkdChangeNotification.Fields.FiasAddress.Edit", "Адрес");
            this.Permission("GkhGji.MkdChangeNotification.Fields.NotificationCause.Edit", "Причина уведомления");
            this.Permission("GkhGji.MkdChangeNotification.Fields.InboundNumber.Edit", "Номер");
            this.Permission("GkhGji.MkdChangeNotification.Fields.RegistrationDate.Edit", "Дата регистрации уведомления");
            this.Permission("GkhGji.MkdChangeNotification.Fields.OldMkdManagementMethod.Edit", "Предыдущий способ управления");
            this.Permission("GkhGji.MkdChangeNotification.Fields.OldManagingOrganization.Edit", "Предыдущий способ управления - УО");
            this.Permission("GkhGji.MkdChangeNotification.Fields.NewMkdManagementMethod.Edit", "Новый способ управления");
            this.Permission("GkhGji.MkdChangeNotification.Fields.NewManagingOrganization.Edit", "Новый способ управления - УО");

            #endregion Справочники

            #region Отчеты
            this.Permission("Reports.GJI.JurPersonInspectionPlan", "План проведения проверок ЮЛ/ИП");
            this.Permission("Reports.GJI.ActReviseInspectionHalfYear", "Акт сверки плановых проверок за полугодие года");
            this.Permission("Reports.GJI.NoActionsMadeListPrescriptions", "Список предписаний, по которым не выполнены мероприятия");
            this.Permission("Reports.GJI.NsoBusinessActivityReport", "Реестр уведомлений о начале предпринимательской деятельности (НСО)");

            #endregion

            #region Приказ Поля
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.PeriodCorrect_View", "Срок проверки - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.ProsecutorDecDate_View", "Дата решения прокурора - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.ProsecutorDecNumber_View", "Номер решения прокурора - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.FactViols_View", "Факты нарушений - Просмотр");
            #endregion

            #region Акт проверки
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.NotRevealedViolations_View", "Нарушения не выявлены - Просмотр");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.PersonsWhoHaveViolated_View", "Сведения о лицах - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.PersonsWhoHaveViolated_Edit", "Сведения о лицах - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.OfficialsGuiltyActions_View", "Сведения свидетельствующие - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.OfficialsGuiltyActions_Edit", "Сведения свидетельствующие - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.ViolationDescription_View", "Описание нарушения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.ViolationDescription_Edit", "Описание нарушения - Редактирование");
            #endregion Акт проверки


            #region Поля проверяемые дома
            this.Namespace("GkhGji.Inspection.BaseJurPerson.Register.RealityObject.Column", "Столбцы");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Register.RealityObject.Column.RoomNums", "Номер квартиры");

            this.Namespace("GkhGji.Inspection.BaseDispHead.Register.RealityObject.Column", "Столбцы");
            this.Permission("GkhGji.Inspection.BaseDispHead.Register.RealityObject.Column.RoomNums", "Номер квартиры");

            this.Namespace("GkhGji.Inspection.BaseProsClaim.Register.RealityObject.Column", "Столбцы");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Register.RealityObject.Column.RoomNums", "Номер квартиры");

            this.Namespace("GkhGji.Inspection.BaseStatement.Register.RealityObject.Column", "Столбцы");
            this.Permission("GkhGji.Inspection.BaseStatement.Register.RealityObject.Column.RoomNums", "Номер квартиры");

            #endregion

            #region Предписание
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentPlace_View", "Место составления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentPlace_Edit", "Место составления - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentTime_View", "Время составления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentTime_Edit", "Время составления - Редактирование");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.BaseDocument", "Деятельность");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.BaseDocument.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.BaseDocument.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.BaseDocument.Delete", "Удаление записей");

            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Violation.Create", "Добавление записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Violation.Delete", "Удаление записей");
            #endregion

            #region Протокол
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Position_Edit", "Должность - Редактирование");
            #endregion

            this.Namespace<AppealCits>("GkhGji.AppealCitizensState.Request", "Запросы в компетентные организации");
            this.Permission("GkhGji.AppealCitizensState.Request.View", "Просмотр");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.SurveySubject", "Предметы проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveySubject.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveySubject.Delete", "Удаление записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveySubject.ShowActual", "Показывать актуальные предметы проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveySubject.ShowNotActual", "Показывать неактуальные предметы проверки");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.SurveyPurpose", "Цели проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyPurpose.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyPurpose.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.SurveyObjective", "Задачи проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyObjective.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyObjective.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.InspFoundation", "НПА требований");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundation.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundation.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheck", "НПА проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheck.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheck.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.AdminRegulation", "Административные регламенты");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.AdminRegulation.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.AdminRegulation.Delete", "Удаление записей");

            this.Namespace<ActCheck>("GkhGji.DocumentsGji.ActCheck", "Акты проверки");
            this.Permission("GkhGji.DocumentsGji.ActCheck.MergeActs", "Объединить акты");

            this.Namespace<ActRemoval>("GkhGji.DocumentsGji.ActRemoval", "Акт проверки предписаний");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.MergeActs", "Объединить акты");

            this.Namespace<Protocol197>("GkhGji.DocumentsGji.Protocol197", "Протокол по ст.19.7 КоАП РФ");
            this.Permission("GkhGji.DocumentsGji.Protocol197.View", "Просмотр");
        }
    }
}