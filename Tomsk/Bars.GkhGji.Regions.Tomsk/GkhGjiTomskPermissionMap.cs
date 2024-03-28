namespace Bars.GkhGji.Regions.Tomsk
{
    using B4;
    using Entities;
	using GkhGji.Entities;
    using Controller;

    public class GkhGjiTomskPermissionMap : PermissionMap
    {
        public GkhGjiTomskPermissionMap()
        {
            #region Управление задачами
            this.Namespace("GkhGji.ManagementTask.ReminderAppealCits", "Задачи по обращениям");
            this.Permission("GkhGji.ManagementTask.ReminderAppealCits.View", "Просмотр");
            #endregion

            this.Namespace("GkhGji.Dict.FrameVerification", "Рамки проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.FrameVerification");

            this.Namespace("GkhGji.Dict.SocialStatus", "Социальные положения");
            this.CRUDandViewPermissions("GkhGji.Dict.SocialStatus");

            this.Permission("GkhGji.AppealCitizensState.Field.SocialStatus_View", "Социальное положение - Просмотр");

            #region Акт визуального осмотра

            this.Namespace<ActVisual>("GkhGji.DocumentsGji.ActVisual", "Акты визуального осмотра");
            this.Permission("GkhGji.DocumentsGji.ActVisual.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActVisual.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ActVisual.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.ActVisual.Field.DocumentNumber_Edit", "Номер документа - Изменение");
            this.Permission("GkhGji.DocumentsGji.ActVisual.Field.DocumentDate_Edit", "Дата документа - Изменение");
            this.Permission("GkhGji.DocumentsGji.ActVisual.Field.DocumentTime_Edit", "Время составления - Изменение");

            this.Namespace("GkhGji.DocumentsGji.ActVisual.Details", "Реквизиты");
            this.Permission("GkhGji.DocumentsGji.ActVisual.Details.Inspectors_Edit", "Инспекторы - Изменение");
            this.Permission("GkhGji.DocumentsGji.ActVisual.Details.Address_Edit", "Адрес - Изменение");
            this.Permission("GkhGji.DocumentsGji.ActVisual.Details.Flat_Edit", "Квартира - Изменение");
            this.Permission("GkhGji.DocumentsGji.ActVisual.Details.CheckArea_Edit", "Рамки проверки - Изменение");

            this.Namespace("GkhGji.DocumentsGji.ActVisual.CheckResult", "Результат проверки");
            this.Permission("GkhGji.DocumentsGji.ActVisual.CheckResult.CheckResult_Edit", "Результат проверки - Изменение");

            this.Namespace("GkhGji.DocumentsGji.ActVisual.Conclusion", "Вывод");
            this.Permission("GkhGji.DocumentsGji.ActVisual.Conclusion.Conclusion_Edit", "Вывод - Изменение");

            #endregion

            #region Административное дело

            this.Namespace<AdministrativeCase>("GkhGji.DocumentsGji.AdminCase", "Реестр административных дел");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.AdminCase");
            //this.Permission("GkhGji.DocumentsGji.AdminCase.Delete", "Удаление записей");
           
            this.Namespace("GkhGji.DocumentsGji.AdminCase.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Field.DocumentNumber_Edit", "Номер документа");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Field.DocumentDate_Edit", "Дата документа - Изменение");

            this.Namespace("GkhGji.DocumentsGji.AdminCase.Details", "Реквизиты");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.TypeAdminCaseBase_View", "Основание - Просмотр");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.TypeAdminCaseBase_Edit", "Основание - Изменение");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.Inspector_View", "Инспектор - Просмотр");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.Inspector_Edit", "Инспектор - Изменение");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.ParentDocument_View", "Родительский документ - Просмотр");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.RealityObject_View", "Объект недвижимости - Просмотр");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.RealityObject_Edit", "Объект недвижимости - Изменение");

            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.Contragent_View", "Контрагент - Просмотр");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.Contragent_Edit", "Контрагент - Изменение");

            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.DescriptionQuestion_View", "Вопрос - Просмотр");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.DescriptionQuestion_Edit", "Вопрос - Изменение");

            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.DescriptionSet_View", "Установил - Просмотр");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.DescriptionSet_Edit", "Установил - Изменение");

            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.DescriptionDefined_View", "Определил - Просмотр");
            this.Permission("GkhGji.DocumentsGji.AdminCase.Details.DescriptionDefined_Edit", "Определил - Изменение");

            this.Namespace("GkhGji.DocumentsGji.AdminCase.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.AdminCase.Register.ArticleLaw", "Статьи закона");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.AdminCase.Register.ArticleLaw");

            this.Namespace("GkhGji.DocumentsGji.AdminCase.Register.Docs", "Документы");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.AdminCase.Register.Docs");

            this.Namespace("GkhGji.DocumentsGji.AdminCase.Register.ProvidedDocs", "Предоставляемые документы");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.AdminCase.Register.ProvidedDocs");

            this.Namespace("GkhGji.DocumentsGji.AdminCase.Register.Annex", "Приложения");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.AdminCase.Register.Annex");

            #endregion Административное дело

            #region Постановление - поля

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentTime_Edit", "Время составления документа");

            #endregion 


            this.Permission("Reports.GJI.AppealCitsWorkingReport", "Журнал учета переданных в работу обращений");

            this.Permission("GkhGji.AppealCitizensState.Field.SpecialControl_View", "Особый контроль - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.SpecialControl_Edit", "Особый контроль - Редактирование");

			#region Обращение - Рассмотрение

			this.Namespace<AppealCits>("GkhGji.AppealCitizensState.Consideration", "Рассмотрение");
			this.Namespace<AppealCits>("GkhGji.AppealCitizensState.Consideration.Executants", "Исполнители");
			this.Permission("GkhGji.AppealCitizensState.Consideration.Executants.Create", "Добавление");
			this.Permission("GkhGji.AppealCitizensState.Consideration.Executants.Delete", "Удаление");
			this.Permission("GkhGji.AppealCitizensState.Consideration.Executants.Edit", "Изменение");

			this.Permission("GkhGji.AppealCitizensState.Field.Consideration.Comment_View", "Комментарий - просмотр");
			this.Permission("GkhGji.AppealCitizensState.Field.Consideration.SuretyResolve_View", "Резолюция - просмотр");

			#endregion

			this.Namespace("GkhGji.Dict.SurveySubject", "Предметы проверки");
			this.CRUDandViewPermissions("GkhGji.Dict.SurveySubject");
			
			this.Namespace("GkhGji.Dict.AnnexToAppealForLicenseIssuance", "Приложения к обращению за выдачей лицензии");
			this.CRUDandViewPermissions("GkhGji.Dict.AnnexToAppealForLicenseIssuance");
        }
    }
}
