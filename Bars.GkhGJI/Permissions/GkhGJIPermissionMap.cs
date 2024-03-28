namespace Bars.GkhGji.Permissions
{
    using B4;
    using Bars.B4.Application;
    using Bars.Gkh.TextValues;
    using Bars.GkhGji.Contracts;
    using Entities;

    /// <summary>
    /// PermissionMap для GkhGjiPermissionMap
    /// </summary>
    public class GkhGjiPermissionMap : PermissionMap
    {
        /// <summary>
        /// Интерфейс для описания текстовых значений пунктов меню
        /// </summary>
        public IMenuItemText MenuItemText { get; set; }

        /// <summary>
        /// Конструктор GkhGjiPermissionMap
        /// </summary>
        public GkhGjiPermissionMap()
        {
            this.Namespace("GkhGji", "Модуль ГЖИ");

            #region Виджеты
            this.Permission("Widget.TaskState", "Состояние задач");
            this.Permission("Widget.TaskControl", "Контроль задач");
            this.Permission("Widget.TaskTable", "Доска задач");

            #endregion

            #region Участники процесса 
            this.Namespace("Gkh.Orgs", "Участники процесса");

            this.Namespace("Gkh.Orgs.SpecAccOwner", "Владелец спецсчета");
            this.CRUDandViewPermissions("Gkh.Orgs.SpecAccOwner");
            #endregion

            #region Уведомление о начале предпринимательской деятельности
            this.Namespace("GkhGji.BusinessActivityViewCreate", "Уведомление о начале предпринимательской деятельности: Просмотр, создание");
            this.Permission("GkhGji.BusinessActivityViewCreate.View", "Просмотр");

            this.Permission("GkhGji.BusinessActivityViewCreate.Create", "Создание записей");

            this.Namespace<BusinessActivity>("GkhGji.BusinessActivity", "Уведомление о начале предпринимательской деятельности: Изменение, удаление");
            this.Permission("GkhGji.BusinessActivity.Edit", "Изменение записей");
            this.Permission("GkhGji.BusinessActivity.Delete", "Удаление записей");

            #region Уведомление о начале предпринимательской деятельности - поля
            this.Namespace<BusinessActivity>("GkhGji.BusinessActivity.Field", "Поля");
            this.Permission("GkhGji.BusinessActivity.Field.Registering", "Регистрация");
            this.Permission("GkhGji.BusinessActivity.Field.Contragent_Edit", "Контрагент");
            this.Permission("GkhGji.BusinessActivity.Field.OrganizationFormName_Edit", "Организационно-правовая форма");
            this.Permission("GkhGji.BusinessActivity.Field.Ogrn_Edit", "ОГРН");
            this.Permission("GkhGji.BusinessActivity.Field.Inn_Edit", "ИНН");
            this.Permission("GkhGji.BusinessActivity.Field.MailingAddress_Edit", "Почтовый адрес");
            this.Permission("GkhGji.BusinessActivity.Field.TypeKindActivity_Edit", "Вид деятельности");
            this.Permission("GkhGji.BusinessActivity.Field.IncomingNotificationNum_Edit", "Входящий номер управления");
            this.Permission("GkhGji.BusinessActivity.Field.DateBegin_Edit", "Дата начала деятельности");
            this.Permission("GkhGji.BusinessActivity.Field.DateRegistration_Edit", "Дата регистрации");
            this.Permission("GkhGji.BusinessActivity.Field.IsNotBuisnes_Edit", "Не осуществляет предпринимательскую деятельность");
            this.Permission("GkhGji.BusinessActivity.Field.AcceptedOrganization_Edit", "Орган, принявший уведомление");
            this.Permission("GkhGji.BusinessActivity.Field.RegNum_Edit", "Регистрационный номер");
            this.Permission("GkhGji.BusinessActivity.Field.IsOriginal_Edit", "Оригинал");
            this.Permission("GkhGji.BusinessActivity.Field.File_Edit", "Файл");
            this.Permission("GkhGji.BusinessActivity.Field.DateNotif_Edit", "Дата уведомления");
            #endregion Уведомление о начале предпринимательской деятельности - поля

            #region Уведомление о начале предпринимательской деятельности - реестры

            this.Namespace("GkhGji.BusinessActivity.Register", "Реестры");

            this.Namespace("GkhGji.BusinessActivity.Register.ServiceJuridal", "Услуги (работы), оказываемые юридическим лицом");
            this.CRUDandViewPermissions("GkhGji.BusinessActivity.Register.ServiceJuridal");
            #endregion Уведомление о начале предпринимательской деятельности - реестры

            #endregion Уведомление о начале предпринимательской деятельности

            #region Проверки

            this.Namespace("GkhGji.Inspection", "Проверки");

            #region Плановые проверки юр.лиц

            this.Namespace("GkhGji.Inspection.BaseJurPerson", "Плановые проверки юр.лиц");
            this.CRUDandViewPermissions("GkhGji.Inspection.BaseJurPerson");
            this.Permission("GkhGji.Inspection.BaseJurPerson.CheckBoxShowCloseInsp", "Показать закрытые проверки");
            

            #region Плановые проверки юр.лиц - поля

            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseJurPerson.Field", "Поля");

            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.Plan_Edit", "План");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.UriRegistrationNumber_Edit", "Учетный номер проверки в едином реестре проверок");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.UriRegistrationDate_Edit", "Дата присвоения учетного номера");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.DateStart_Edit", "Дата начала проверки");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.InsNumber_Edit", "Номер");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.CountDays_Edit", "Срок проверки (количество дней)");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.TypeBaseJuralPerson_Edit", "Основание проверки");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.TypeFact_Edit", "Факт проверки");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.Reason_Edit", "Причина");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.TypeForm_Edit", "Форма проверки");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.ZonalInspections_Edit", "Отделы");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.ReasonErpChecking_View", "Основание для включения проверки в ЕРП - просмотр");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.ReasonErpChecking_Edit", "Основание для включения проверки в ЕРП - изменение");

            #endregion Плановые проверки юр.лиц - поля

            #region Плановые проверки юр.лиц - реестры

            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseJurPerson.Register", "Реестры");

            this.Namespace("GkhGji.Inspection.BaseJurPerson.Register.RealityObject", "Проверяемые дома");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Register.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Register.RealityObject.Delete", "Удаление записей");

            #endregion Плановые проверки юр.лиц - реестры

            #endregion Плановые проверки юр.лиц

            #region Инспекционные проверки

            this.Namespace("GkhGji.Inspection.BaseInsCheck", "Инспекционные проверки");
            this.CRUDandViewPermissions("GkhGji.Inspection.BaseInsCheck");
            this.Permission("GkhGji.Inspection.BaseInsCheck.CheckBoxShowCloseInsp", "Показать закрытые проверки");

            #region Инспекционные проверки - поля
            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseInsCheck.Field", "Поля");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.Plan_Edit", "План");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.InsCheckDate_Edit", "Дата");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.InsNumber_Edit", "Номер");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.DateStart_Edit", "Дата начала");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.Area_Edit", "Площадь");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.Reason_Edit", "Причина");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.TypeFact_Edit", "Факт проверки");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.RealityObject_Edit", "Жилой дом");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.ReasonErpChecking_View", "Основание для включения проверки в ЕРП - Просмотр");
            this.Permission("GkhGji.Inspection.BaseInsCheck.Field.ReasonErpChecking_Edit", "Основание для включения проверки в ЕРП - Редактирование");
            #endregion Инспекционные проверки - поля

            #endregion Инспекционные проверки

            #region Проверки по плану мероприятий

            this.Namespace<BasePlanAction>("GkhGji.Inspection.BasePlanAction", "Проверки по плану мероприятий");
            this.CRUDandViewPermissions("GkhGji.Inspection.BasePlanAction");

            #endregion Проверки по плану мероприятий

            #region Проверки по поручению руководства

            this.Namespace("GkhGji.Inspection.BaseDispHead", "Проверки по поручению руководства");
            this.CRUDandViewPermissions("GkhGji.Inspection.BaseDispHead");
            this.Permission("GkhGji.Inspection.BaseDispHead.CheckBoxShowCloseInsp", "Показать закрытые проверки");

            #region Проверки по поручению руководства - поля

            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseDispHead.Field", "Поля");

            this.Permission("GkhGji.Inspection.BaseDispHead.Field.DispHeadDate_Edit", "Дата");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.InspectionNumber_Edit", "Номер");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.Head_Edit", "Руководитель");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.TypeBaseDispHead_Edit", "Основание");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.PrevDocument_Edit", "Предыдущий документ");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.TypeForm_Edit", "Форма проверки");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.DocumentNumber_Edit", "Номер документа");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.DocumentName_Edit", "Наименование документа");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.DocumentDate_Edit", "Дата документа");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.TypeJurPerson_Edit", "Тип юридического лица");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.PersonInspection_Edit", "Объект проверки");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.Contragent_Edit", "Юридическое лицо");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.PhysicalPerson_Edit", "ФИО");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.ReasonErpChecking_View", "Основание для включения проверки в ЕРП - Просмотр");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.ReasonErpChecking_Edit", "Основание для включения проверки в ЕРП - Редактирование");

            #endregion Проверки по поручению руководства - поля

            #region Проверки по поручению руководства - реестры

            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseDispHead.Register", "Реестры");

            this.Namespace("GkhGji.Inspection.BaseDispHead.Register.RealityObject", "Проверяемые дома");
            this.Permission("GkhGji.Inspection.BaseDispHead.Register.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseDispHead.Register.RealityObject.Delete", "Удаление записей");

            #endregion Проверки по поручению руководства - реестры

            #endregion Проверки по поручению руководства

            #region Проверки по требованию прокуратуры

            this.Namespace("GkhGji.Inspection.BaseProsClaim", "Проверки по требованию прокуратуры");
            this.CRUDandViewPermissions("GkhGji.Inspection.BaseProsClaim");
            this.Permission("GkhGji.Inspection.BaseProsClaim.CheckBoxShowCloseInsp", "Показать закрытые проверки");

            #region Проверки по требованию прокуратуры - поля

            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseProsClaim.Field", "Поля");

            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.ProsClaimDate_Edit", "Дата");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.InspectionNumber_Edit", "Номер");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.TypeBaseProsClaim_Edit", "Тип проверки");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.ProsClaimDateCheck_Edit", "Дата проверки");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.IssuedClaim_Edit", "ДЛ, вынесшее требование");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.TypeForm_Edit", "Форма проверки");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.DocumentName_Edit", "Наименование документа");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.DocumentNumber_Edit", "Номер документа");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.DocumentDate_Edit", "Дата документа");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.DocumentDescription_Edit", "Описание документа");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.TypeJurPerson_Edit", "Тип юридического лица");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.PersonInspection_Edit", "Объект проверки");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.Contragent_Edit", "Юридическое лицо");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.ReasonErpChecking_View", "Основание для включения проверки в ЕРП - Просмотр");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.ReasonErpChecking_Edit", "Основание для включения проверки в ЕРП - Редактирование");

            #endregion Проверки по требованию прокуратуры - поля

            #region Проверки по требованию прокуратуры - реестры
            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseProsClaim.Register", "Реестры");

            this.Namespace("GkhGji.Inspection.BaseProsClaim.Register.RealityObject", "Проверяемые дома");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Register.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Register.RealityObject.Delete", "Удаление записей");

            #endregion Проверки по требованию прокуратуры - реестры

            #endregion Проверки по требованию прокуратуры

            #region Проверки по обращениям граждан
            this.Namespace("GkhGji.Inspection.BaseStatement", "Проверки по обращениям граждан");
            this.CRUDandViewPermissions("GkhGji.Inspection.BaseStatement");
            this.Permission("GkhGji.Inspection.BaseStatement.CheckBoxShowCloseInsp", "Показать закрытые проверки");

            #region Проверки по обращениям граждан - поля
            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseStatement.Field", "Поля");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.TypeJurPerson_Edit", "Тип юридического лица");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.PersonInspection_Edit", "Объект проверки");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.Contragent_Edit", "Юридическое лицо");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.InspectionNumber_Edit", "Номер");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.ReasonErpChecking_View", "Основание для включения проверки в ЕРП - Просмотр");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.ReasonErpChecking_Edit", "Основание для включения проверки в ЕРП - Редактирование");

            #endregion Проверки по обращениям граждан - поля

            #region Проверки по обращениям граждан - реестры
            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseStatement.Register", "Реестры");

            this.Namespace("GkhGji.Inspection.BaseStatement.Register.RealityObject", "Проверяемые дома");
            this.Permission("GkhGji.Inspection.BaseStatement.Register.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseStatement.Register.RealityObject.Delete", "Удаление записей");

            #endregion Проверки по обращениям граждан - реестры

            this.Permission("GkhGji.Inspection.BaseStatement.Field.Consideration.SuretyInspection_Edit", "Проверка в отношении");
            #endregion Проверки по обращениям граждан

            this.Namespace("GkhGji.Inspection.BaseLicApplicants", "Проверки соискателей лицензии");
            this.CRUDandViewPermissions("GkhGji.Inspection.BaseLicApplicants");
            this.Permission("GkhGji.Inspection.BaseLicApplicants.CheckBoxShowCloseInsp", "Показать закрытые проверки");
            this.Namespace<InspectionGji>("GkhGji.Inspection.BaseLicApplicants.Register", "Реестры");
            this.Namespace("GkhGji.Inspection.BaseLicApplicants.Register.RealityObject", "Проверяемые дома");
            this.Permission("GkhGji.Inspection.BaseLicApplicants.Register.RealityObject.ShowAll_View", "Показывать все дома");
            this.Namespace("GkhGji.Inspection.BaseLicApplicants.Field", "Поля");
            this.Permission("GkhGji.Inspection.BaseLicApplicants.Field.ReasonErpChecking_View", "Основание для включения проверки в ЕРП - Просмотр");
            this.Permission("GkhGji.Inspection.BaseLicApplicants.Field.ReasonErpChecking_Edit", "Основание для включения проверки в ЕРП - Редактирование");
            
            #region Проверки без основания
            this.Namespace("GkhGji.Inspection.BaseDefault", "Проверки без основания");

            this.Permission("GkhGji.Inspection.BaseDefault.View", "Просмотр");
            this.Permission("GkhGji.Inspection.BaseDefault.CheckBoxShowCloseInsp", "Показать закрытые проверки");

            this.Namespace("GkhGji.Inspection.BaseDefault.Field", "Поля");
            this.Permission("GkhGji.Inspection.BaseDefault.Field.InspectionNumber_Edit", "Номер");
            this.Permission("GkhGji.Inspection.BaseDefault.Field.ReasonErpChecking_View", "Основание для включения проверки в ЕРП - Просмотр");
            this.Permission("GkhGji.Inspection.BaseDefault.Field.ReasonErpChecking_Edit", "Основание для включения проверки в ЕРП - Редактирование");
            
            #endregion

            #endregion Проверки

            #region Документы ГЖИ
            this.Namespace<DocumentGji>("GkhGji.DocumentsGji", "Документы ГЖИ");
            this.Permission("GkhGji.DocumentsGji.View", "Просмотр");

            #region Решения
            this.Namespace<Decision>("GkhGji.DocumentsGji.Decision", "Решения");
            this.Permission("GkhGji.DocumentsGji.Decision.View", "Просмотр");
            #endregion Решения

            #region Распоряжения

            var disposalText = ApplicationContext.Current.Container.Resolve<IDisposalText>();

            this.Namespace<Disposal>("GkhGji.DocumentsGji.Disposal", disposalText.SubjectiveManyCase);
            this.Permission("GkhGji.DocumentsGji.Disposal.View", "Просмотр");

            #region Распоряжения - поля
            this.Namespace("GkhGji.DocumentsGji.Disposal.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.ResponsibleExecution_Edit", "Ответственный за исполнение");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DateStart_Edit", "Период с");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DateEnd_Edit", "Период по");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.OutInspector_View", "Выезд инспектора в командировку - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.OutInspector_Edit", "Выезд инспектора в командировку - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.ObjectVisitStart_View", "Выезд на объект с - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.ObjectVisitStart_Edit", "Выезд на объект с - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.ObjectVisitEnd_View", "Выезд на объект по - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.ObjectVisitEnd_Edit", "Выезд на объект по - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.KindCheck_Edit", "Вид проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.TypeAgreementProsecutor_Edit", "Согласование с прокуратурой");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.TypeAgreementResult_Edit", "Результат согласования");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.IssuedDisposal_Edit", "ДЛ, вынесшее распоряжение");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.Inspectors_Edit", "Инспекторы");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.Notice_Fieldset_View", "Уведомление о проверке - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.Notice_Fieldset_Edit", "Уведомление о проверке - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentNumberWithResultAgreement_View", "Номер документа с результатом согласования - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentNumberWithResultAgreement_Edit", "Номер документа с результатом согласования - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentDateWithResultAgreement_View", "Дата документа с результатом согласования - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.DocumentDateWithResultAgreement_Edit", "Дата документа с результатом согласования - Редактирование");

            #endregion Распоряжения - поля

            #region Распоряжения - реестры
            this.Namespace("GkhGji.DocumentsGji.Disposal.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.TypeSurvey", "Типы обследования");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.TypeSurvey.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.TypeSurvey.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.Expert", "Эксперты");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.Expert.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.Expert.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.ProvidedDoc", "Предоставляемые документы");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.ProvidedDoc.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.ProvidedDoc.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.Annex.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.Annex.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.Notice", "Уведомление о проверке");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.Notice.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.Notice.Edit", "Редактирование");

            #endregion Распоряжения - реестры

            #endregion Распоряжения

            #region Акты проверки
            this.Namespace<ActCheck>("GkhGji.DocumentsGji.ActCheck", "Акты проверки");
            this.Permission("GkhGji.DocumentsGji.ActCheck.View", "Просмотр");

            this.Permission("GkhGji.DocumentsGji.ActCheck.CreateResolutionRospotrebnadzor_View", "Сформировать Постановление Роспотребнадзора - Просмотр");

            this.Permission("GkhGji.DocumentsGji.ActCheck.ResolutionRospotrebnadzor_View", "Требуется направление в Роспотребнадзор - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.ResolutionRospotrebnadzor_Edit", "Требуется направление в Роспотребнадзор - Редактирование");

            #region Акты проверки - поля
            this.Namespace("GkhGji.DocumentsGji.ActCheck.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_View", "Место составления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_Edit", "Место составления - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentTime_View", "Время составления акта - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentTime_Edit", "Время составления акта - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentDate_Edit", "Дата документа");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.Area_Edit", "Площадь - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.Area_View", "Площадь - Просмотр");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.Flat_Edit", "Квартира - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.Flat_View", "Квартира - Просмотр");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.ToProsecutor_Edit", "Передано в прокуратуру");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DateToProsecutor_Edit", "Дата передачи");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.ResolutionProsecutor_Edit", "Постановление прокуратуры");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.Inspectors_View", "Инспекторы - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.Inspectors_Edit", "Инспекторы - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedWithDisposalCopy_View", "С копией приказа ознакомлен - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedWithDisposalCopy_Edit", "С копией приказа ознакомлен - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_View", "Место составления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentPlace_Edit", "Место составления - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentTime_View", "Время составления акта - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.DocumentTime_Edit", "Время составления акта - Редактирование");
            #endregion Акты проверки - поля

            #region Акты проверки - реестры
            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Requisites", "Реквизиты");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Requisites.View", "Просмотр");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Witness", "Лица, присутствующие при проверке");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Witness.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Witness.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Witness.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Witness.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Period", "Дата и время проверки");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Period.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Period.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Period.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Period.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Violation", "Результаты проверки (Нарушения)");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.View", "Просмотр записей (Новые нарушения)");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Violation.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Definition", "Определения");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Definition.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Definition.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Definition.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Definition.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Annex.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Annex.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Annex.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.InspectedPart", "Инспектируемые части");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.InspectedPart.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc", "Предоставленные документы");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.ProvidedDoc.Delete", "Удаление записей");

            #endregion Акты проверки - реестры

            #endregion Акты проверки

            #region Акты обследования
            this.Namespace<ActSurvey>("GkhGji.DocumentsGji.ActSurvey", "Акты обследования");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.View", "Просмотр");

            #region Акты обследования - поля
            this.Namespace("GkhGji.DocumentsGji.ActSurvey.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.DocumentDate_Edit", "Дата");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.Flat_Edit", "Квартира");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.Area_Edit", "Обследованная площадь");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.FactSurveyed_Edit", "Факт обследования");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.Reason_Edit", "Причина");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Field.Description_Edit", "Описание (Выводы по результату)");

            #endregion Акты обследования - поля

            #region Акты обследования - реестры
            this.Namespace("GkhGji.DocumentsGji.ActSurvey.Register", "Реестры");

            /* Права по данному рестр вынесены в отдельный файл чтобы в регионе Саха заменит ьи перемеиновать пермишены
            Namespace("GkhGji.DocumentsGji.ActSurvey.Register.Owner", "Собственники");
            Permission("GkhGji.DocumentsGji.ActSurvey.Register.Owner.Edit", "Изменение записей");
            Permission("GkhGji.DocumentsGji.ActSurvey.Register.Owner.Create", "Создание записей");
            Permission("GkhGji.DocumentsGji.ActSurvey.Register.Owner.Delete", "Удаление записей");
            */

            this.Namespace("GkhGji.DocumentsGji.ActSurvey.Register.InspectedPart", "Инспектируемые части");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Register.InspectedPart.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Register.InspectedPart.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Register.InspectedPart.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ActSurvey.Register.Photo", "Фото");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Register.Photo.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Register.Photo.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Register.Photo.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ActSurvey.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Register.Annex.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ActSurvey.Register.Annex.Delete", "Удаление записей");

            #endregion Акты обследования - реестры

            #endregion Акты обследования

            #region Акты проверки предписаний
            this.Namespace<ActRemoval>("GkhGji.DocumentsGji.ActRemoval", "Акт проверки предписаний");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.View", "Просмотр");

            #region Акты проверки предписаний - поля
            this.Namespace("GkhGji.DocumentsGji.ActRemoval.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DocumentDate_Edit", "Дата документа");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.Area_Edit", "Площадь");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.Flat_Edit", "Квартира");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.TypeRemoval_Edit", "Нарушения устранены");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DateFactRemoval_Edit", "Дата фактического исполнения");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.Description_Edit", "Описание");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.DatePlanRemoval_Edit", "Срок устранения");

            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.CircumstancesDescription_View", "Описание обстоятельств - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.CircumstancesDescription_Edit", "Описание обстоятельств - Редактирование");

            #endregion Акты проверки предписаний - поля

            #endregion Акты проверки предписаний

            #region Акты профилактического визита
            this.Namespace<PreventiveVisit>("GkhGji.DocumentsGji.PreventiveVisit", "Акт профилактического визита");
            this.Permission("GkhGji.DocumentsGji.PreventiveVisit.View", "Просмотр");
            #endregion Акты профилактического визита

            #region Предписания
            this.Namespace<Prescription>("GkhGji.DocumentsGji.Prescription", "Предписания");
            this.Permission("GkhGji.DocumentsGji.Prescription.View", "Просмотр");

            #region Предписания - поля
            this.Namespace("GkhGji.DocumentsGji.Prescription.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Prescription.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Prescription.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.Description_Edit", "Примечание");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.Executant_Edit", "Тип исполнителя");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.Contragent_Edit", "Контрагент");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.PhysicalPerson_Edit", "Физическое лицо");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.PhysicalPersonInfo_Edit", "Реквизиты физического лица");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Field.Close", "Закрытие");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.Close.Closed_Edit", "Предписание закрыто - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.Close.Reason_Edit", "Причина - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Prescription.Field.Close.Note_Edit", "Примечание - Редактирование");

            #endregion Предписания - поля

            #region Предписания - реестры
            this.Namespace("GkhGji.DocumentsGji.Prescription.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.Requisites", "Реквизиты");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Requisites.View", "Просмотр");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.Violation", "Нарушения");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Violation.View", "Просмотр записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Violation.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Violation.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Violation.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.Violation.Fields", "Поля");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Violation.Fields.DateFactRemoval_Edit", "Дата факт. исполнения - Редактирование");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.ArticleLaw", "Статьи закона");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.View", "Просмотр записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.ArticleLaw.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.Cancel", "Решения предписания");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.View", "Просмотр записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Cancel.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.Close", "Закрытие предписания");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Close.View", "Просмотр раздела");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.Close.Docs", "Документы закрытия предписания");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Close.Docs.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Close.Docs.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Close.Docs.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Annex.View", "Просмотр записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Annex.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Annex.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Prescription.Register.Directions", "Деятельность");
            this.Permission("GkhGji.DocumentsGji.Prescription.Register.Directions.View", "Просмотр записей");

            #endregion Предписаний - реестры

            #endregion Предписания

            #region Протоколы
            this.Namespace<Protocol>("GkhGji.DocumentsGji.Protocol", "Протоколы");
            this.Permission("GkhGji.DocumentsGji.Protocol.View", "Просмотр");

            #region Протокол - поля
            this.Namespace("GkhGji.DocumentsGji.Protocol.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Protocol.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.ToCourt_Edit", "Документы переданы в суд");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DateToCourt_Edit", "Дата передачи в суд");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Description_Edit", "Примечание");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Executant_Edit", "Тип исполнителя");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Contragent_Edit", "Контрагент");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.PhysicalPerson_Edit", "Физическое лицо");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.PhysicalPersonInfo_Edit", "Реквизиты физического лица");

            #endregion Протокол - поля

            #region Протокол - реестры
            this.Namespace("GkhGji.DocumentsGji.Protocol.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.Protocol.Register.Requisites", "Реквизиты");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Requisites.View", "Просмотр записей");

            this.Namespace("GkhGji.DocumentsGji.Protocol.Register.Violation", "Нарушения");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Violation.View", "Просмотр записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Violation.Edit", "Редактирование записей");

            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Violation.View_DateFactRemoval", "Дата факт. исполнения");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Violation.View_DatePlanRemoval", "Срок устранения");

            this.Namespace("GkhGji.DocumentsGji.Protocol.Register.ArticleLaw", "Статьи закона");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.View", "Просмотр записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.ArticleLaw.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Protocol.Register.Definition", "Определения");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Definition.View", "Просмотр записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Definition.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Definition.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Definition.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Protocol.Register.Directions", "Деятельность");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Directions.View", "Просмотр записей");

            this.Namespace("GkhGji.DocumentsGji.Protocol.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Annex.View", "Просмотр записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Annex.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Protocol.Register.Annex.Delete", "Удаление записей");

            #endregion Протокол - реестры

            #endregion Протоколы

            #region Постановления
            this.Namespace<Resolution>("GkhGji.DocumentsGji.Resolution", "Постановления");
            this.Permission("GkhGji.DocumentsGji.Resolution.View", "Просмотр");

            #region Постановления  - поля
            this.Namespace("GkhGji.DocumentsGji.Resolution.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.SectorNumber_View", "Номер участка - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.SectorNumber_Edit", "Номер участка - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DateTransferSSP_Edit", "Дата передачи в ССП - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentNumSSP_Edit", "Номер документа (передача в ССП)");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Contragent_Edit", "Контрагент");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.PhysicalPerson_Edit", "Физическое лицо");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.PhysicalPersonInfo_Edit", "Реквизиты физического лица");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.AbandonReason_View", "Причина аннулирования - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.AbandonReason_Edit", "Причина аннулирования - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DateWriteOut_View", "Дата выписки из ЕГРЮЛ - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DateWriteOut_Edit", "Дата выписки из ЕГРЮЛ - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.TypeTermination_Edit", "Основание прекращения");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentDate_View", "Дата - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentDate_Edit", "Дата - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.resolutionBaseName_View", "Документ-основание - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.resolutionBaseName_Edit", "Документ-основание - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.GisUin_View", "УИН - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.GisUin_Edit", "УИН - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.OffenderWas_View", "Нарушитель явился на рассмотрение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.OffenderWas_Edit", "Нарушитель явился на рассмотрение - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DeliveryDate_View", "Дата вручения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DeliveryDate_Edit", "Дата вручения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.TypeInitiativeOrg_View", "Кем вынесено - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.TypeInitiativeOrg_Edit", "Кем вынесено - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.FineMunicipality_View", "МО получателя штрафа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.FineMunicipality_Edit", "МО получателя штрафа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Official_View", "Должностное лицо - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Official_Edit", "Должностное лицо - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Municipality_View", "Местонахождение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Municipality_Edit", "Местонахождение - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Sanction_View", "Вид санкции - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Sanction_Edit", "Вид санкции - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.TypeTerminationBasement_View", "Основание прекращения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.TypeTerminationBasement_Edit", "Основание прекращения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentNumber_View", "Номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Paided_View", "Штраф оплачен - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Paided_Edit", "Штраф оплачен - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DateTransferSsp_View", "Дата передачи в ССП - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Executant_View", "Тип исполнител - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.RulinFio_View", "ФИО - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.RulinFio_Edit", "ФИО - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.DocumentNumSsp_View", "Номер документа(Санкция) - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.PenaltyAmount_View", "Сумма штрафа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.PenaltyAmount_Edit", "Сумма штрафа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.RulingNumber_View", "Номер направления копии постановления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.RulingNumber_Edit", "Номер направления копии постановления - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.RulingDate_View", "Дата направления копии постановления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.RulingDate_Edit", "Дата направления копии постановления - Редактирование");

            #endregion Постановления - поля

            #region Постановления - реестры
            this.Namespace("GkhGji.DocumentsGji.Resolution.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.Resolution.Register.Dispute", "Оспаривания");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.Dispute.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.Dispute.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.Dispute.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Resolution.Register.Definition", "Определения");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.Definition.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.Definition.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.Definition.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Resolution.Register.PayFine", "Оплаты штрафов");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.PayFine.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.PayFine.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.PayFine.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Resolution.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.Annex.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Resolution.Register.Annex.Delete", "Удаление записей");

            #endregion Постановления - реестры

            #endregion Постановления

            #region Постановления Роспотребнадзора
            this.Namespace<ResolutionRospotrebnadzor>("GkhGji.DocumentsGji.ResolutionRospotrebnadzor", "Постановления Роспотребнадзора");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.View", "Просмотр");
            #region Поля
            this.Namespace("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentDate_View", "Дата - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentDate_Edit", "Дата - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentNum_Edit", "Номер - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentReason_View", "Документ-основание - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DocumentReason_Edit", "Документ-основание - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DeliveryDate_View", "Дата вручения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.DeliveryDate_Edit", "Дата вручения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.GisUin_View", "УИН - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.RevocationReason_View", "Причина аннулирования - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.RevocationReason_Edit", "Причина аннулирования - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.TypeInitiativeOrg_View", "Кем вынесено - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.ExpireReason_View", "Основание прекращения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.ExpireReason_Edit", "Основание прекращения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PenaltyAmount_View", "Сумма штрафа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PenaltyAmount_Edit", "Сумма штрафа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.SspDocumentNum_View", "Номер документа (передача в ССП) - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Paided_View", "Штраф оплачен - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Paided_Edit", "Штраф оплачен - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.TransferToSspDate_View", "Дата передачи в ССП - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.TransferToSspDate_Edit", "Дата передачи в ССП - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PhysicalPerson_View", "Физическое лицо - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PhysicalPerson_Edit", "Физическое лицо - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PhysicalPersonInfo_View", "Реквизиты физического лица - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.PhysicalPersonInfo_Edit", "Реквизиты физического лица - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.FineMunicipality_View", "МО получателя штрафа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.FineMunicipality_Edit", "МО получателя штрафа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Official_View", "Должностное лицо - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Official_Edit", "Должностное лицо - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.LocationMunicipality_View", "Местонахождение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.LocationMunicipality_Edit", "Местонахождение - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Sanction_View", "Вид санкции - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Sanction_Edit", "Вид санкции - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Executant_View", "Тип исполнителя - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Executant_Edit", "Тип исполнителя - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Contragent_View", "Контрагент - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Field.Contragent_Edit", "Контрагент - Редактирование");
            #endregion

            #region Реестры
            this.Namespace("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Dispute", "Оспаривания");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Dispute.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Dispute.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Dispute.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Definition", "Определения");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Definition.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Definition.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Definition.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.PayFine", "Оплаты штрафов");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.PayFine.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.PayFine.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.PayFine.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Annex.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Annex.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.ArticleLaw", "Статья закона");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.ArticleLaw.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.ArticleLaw.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.ArticleLaw.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Violation", "Нарушения");
            this.Permission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.Register.Violation.Edit", "Редактирование");
            #endregion

            #endregion

            #region Представления
            this.Namespace<Presentation>("GkhGji.DocumentsGji.Presentation", "Представления");
            this.Permission("GkhGji.DocumentsGji.Presentation.View", "Просмотр");

            #region Представление - поля
            this.Namespace("GkhGji.DocumentsGji.Presentation.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.Presentation.Field.DocumentNumber_Edit", "Номер документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Presentation.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Presentation.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Presentation.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Presentation.Field.TypeInitiativeOrg_Edit", "Кем вынесено");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.Official_Edit", "Должностное лицо");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.Executant_Edit", "Тип исполнителя");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.Contragent_Edit", "Контрагент");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.PhysicalPerson_Edit", "Физическое лицо");
            this.Permission("GkhGji.DocumentsGji.Presentation.Field.PhysicalPersonInfo_Edit", "Реквизиты физического лица");

            #endregion Представление - поля

            #region Представление - реестры
            this.Namespace("GkhGji.DocumentsGji.Presentation.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.Presentation.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.Presentation.Register.Annex.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.Presentation.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Presentation.Register.Annex.Delete", "Удаление записей");

            #endregion Представление - реестры

            #endregion Представления

            #region Протокол МВД
            this.Namespace<ProtocolMvd>("GkhGji.DocumentsGji.ProtocolMvd", "Протоколы МВД");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ProtocolMvd");

            #region Протокол МВД - поля
            this.Namespace("GkhGji.DocumentsGji.ProtocolMvd.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentNumber_View", "Номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_View", "Дата - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DocumentDate_Edit", "Дата - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_View", "Орган МВД, оформивший протокол - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.OrganMvd_Edit", "Орган МВД, оформивший протокол - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DateSupply_View", "Дата поступления в ГЖИ - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DateSupply_Edit", "Дата поступления в ГЖИ - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_View", "Муниципальное образование - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.Municipality_Edit", "Муниципальное образование - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_View", "Дата правонарушения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.DateOffense_Edit", "Дата правонарушения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_View", "Время правонарушения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.TimeOffense_Edit", "Время правонарушения - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.Executant_Edit", "Тип исполнителя");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_View", "Тип исполнителя - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.TypeExecutant_Edit", "Тип исполнителя - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_View", "ФИО нарушителя (полностью) - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPerson_Edit", "ФИО нарушителя (полностью) - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_View", "Дата рождения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.BirthDate_Edit", "Дата рождения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_View", "Место рождения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.BirthPlace_Edit", "Место рождения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_View", "Серия и номер паспорта - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.SerialAndNumber_Edit", "Серия и номер паспорта - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_View", "Дата выдачи - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.IssueDate_Edit", "Дата выдачи - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_View", "Кем выдан - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.IssuingAuthority_Edit", "Кем выдан - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_View", "Фактический адрес проживания - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.PhysicalPersonInfo_Edit", "Фактический адрес проживания - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.Company_View", "Место работы, должность - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.Company_Edit", "Место работы, должность - Редактирование");

            #endregion Протокол МВД - поля

            #region Протокол МВД - реестры
            this.Namespace("GkhGji.DocumentsGji.ProtocolMvd.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.ProtocolMvd.Register.ArticleLaw", "Статьи закона");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Register.ArticleLaw.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Register.ArticleLaw.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Register.ArticleLaw.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ProtocolMvd.Register.RealityObject", "Адреса правонарушения");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Register.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Register.RealityObject.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ProtocolMvd.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Register.Annex.Edit", "Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Register.Annex.Delete", "Удаление записей");

            #endregion Протокол МВД - реестры

            #endregion Протокол МВД

            #region Постановление прокуратуры
            this.Namespace<ResolPros>("GkhGji.DocumentsGji.ResolPros", "Постановления прокуратуры");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ResolPros");

            this.Namespace("GkhGji.DocumentsGji.ResolPros.Documents", "Создание документов");

            this.Permission("GkhGji.DocumentsGji.ResolPros.Documents.Resolution", "Постановление - создание");

            #region Постановление прокуратуры - поля
            this.Namespace("GkhGji.DocumentsGji.ResolPros.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.DocumentNumber_Edit", "Номер документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.Municipality_Edit", "Орган прокуратуры, вынесший постановление");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.DateSupply_Edit", "Дата поступления в ГЖИ");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.ActCheck_Edit", "Акт проверки");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.Executant_Edit", "Тип исполнителя");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.Contragent_Edit", "Контрагент");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.PhysicalPerson_Edit", "Физическое лицо");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Field.PhysicalPersonInfo_Edit", "Реквизиты физического лица");

            #endregion Постановление прокуратуры - поля

            #region Постановление прокуратуры - реестры
            this.Namespace("GkhGji.DocumentsGji.ResolPros.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.ResolPros.Register.ArticleLaw", "Статьи закона");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Register.ArticleLaw.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Register.ArticleLaw.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Register.ArticleLaw.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ResolPros.Register.RealityObject", "Адреса правонарушения");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Register.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Register.RealityObject.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ResolPros.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Register.Annex.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ResolPros.Register.Annex.Delete", "Удаление записей");

            #endregion Постановление прокуратуры - реестры

            #endregion Постановление прокуратуры

            #region Протокол МЖК
            this.Namespace<ProtocolMhc>("GkhGji.DocumentsGji.ProtocolMhc", "Протокол МЖК");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ProtocolMhc");

            this.Namespace<ProtocolRSO>("GkhGji.DocumentsGji.ProtocolRSO", "Протокол РСО");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ProtocolRSO");

            #region Протокол МЖК - поля
            this.Namespace("GkhGji.DocumentsGji.ProtocolMhc.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentNumber_Edit", "Номер документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.Municipality_Edit", "Орган прокуратуры, вынесший постановление");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.DateSupply_Edit", "Дата поступления в ГЖИ");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.Executant_Edit", "Тип исполнителя");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.Contragent_Edit", "Контрагент");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.PhysicalPerson_Edit", "Физическое лицо");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Field.PhysicalPersonInfo_Edit", "Реквизиты физического лица");

            #endregion Протокол МЖК - поля

            #region Протокол РСО - поля
            this.Namespace("GkhGji.DocumentsGji.ProtocolRSO.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentNumber_Edit", "Номер документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.SupplierGas_Edit", "РСО, вынесшее постановление");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.DateSupply_Edit", "Дата поступления в ГЖИ");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.Executant_Edit", "Тип исполнителя");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.Contragent_Edit", "Контрагент");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.PhysicalPerson_Edit", "Физическое лицо");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Field.PhysicalPersonInfo_Edit", "Реквизиты физического лица");
            #endregion

            #region Протокол РСО - реестры
            this.Namespace("GkhGji.DocumentsGji.ProtocolRSO.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.ProtocolRSO.Register.ArticleLaw", "Статьи закона");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.ArticleLaw.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.ArticleLaw.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.ArticleLaw.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ProtocolRSO.Register.RealityObject", "Адреса правонарушения");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.RealityObject.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ProtocolRSO.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.Annex.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.Annex.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ProtocolRSO.Register.Definition", "Определения");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.Definition.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.Definition.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolRSO.Register.Definition.Delete", "Удаление записей");

            #endregion 

            #region Протокол МЖК - реестры
            this.Namespace("GkhGji.DocumentsGji.ProtocolMhc.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.ProtocolMhc.Register.ArticleLaw", "Статьи закона");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.ArticleLaw.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.ArticleLaw.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.ArticleLaw.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ProtocolMhc.Register.RealityObject", "Адреса правонарушения");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.RealityObject.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ProtocolMhc.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.Annex.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.Annex.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.ProtocolMhc.Register.Definition", "Определения");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.Definition.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.Definition.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.ProtocolMhc.Register.Definition.Delete", "Удаление записей");

            #endregion Протоколы МЖК - реестры

            #endregion Протоколы МЖК

            #region Распоряжения вне инспекционной деятельности
            this.Namespace<Disposal>("GkhGji.DocumentsGji.DisposalNullInspection", "Распоряжения вне инспекционной деятельности");
            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.View", "Просмотр");

            this.Namespace("GkhGji.DocumentsGji.DisposalNullInspection.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentNum_Edit", "Номер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.DocumentSubNum_Edit", "Подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.ResponsibleExecution_Edit", "Ответственный за исполнение");
            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.IssuedDisposal_Edit", "ДЛ, вынесшее распоряжение");
            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Field.Description_Edit", "Описание");

            this.Namespace("GkhGji.DocumentsGji.DisposalNullInspection.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.DisposalNullInspection.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Register.Annex.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.DisposalNullInspection.Register.Annex.Delete", "Удаление записей");
            #endregion

            #region Акты без взаимодействия
            this.Namespace<ActIsolated>("GkhGji.DocumentsGji.ActIsolated", "Акты без взаимодействия");
            this.Permission("GkhGji.DocumentsGji.ActIsolated.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Delete", "Удаление");
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Edit", "Изменение");

            #region Акты без взаимодействия - поля
            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Field", "Поля");

            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.DocumentDate_View", "Дата документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.DocumentDate_Edit", "Дата документа - Редактирование");
        
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.DocumentNumber_View", "Номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.DocumentNumber_Edit", "Номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.DocumentNum_Edit", "Номер - Редактирование");
            
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.DocumentYear_Edit", "Год - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActIsolatedField.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActIsolatedField.DocumentSubNum_Edit", "Подномер - Редактирование");   
            #endregion Акты без взаимодействия - поля

            #region Акты без взаимодействия - реестры
            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Register.Requisites", "Реквизиты");
            this.Permission("GkhGji.DocumentsGji.ActIsolated.Register.Requisites.View", "Просмотр");

            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Register.Witness", "Лица, присутствующие при проверке");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActIsolated.Register.Witness");

            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Register.Period", "Дата и время проверки");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActIsolated.Register.Period");

            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Register.Definition", "Определения");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActIsolated.Register.Definition");

            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Register.Annex", "Приложения");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActIsolated.Register.Annex");

            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Register.InspectedPart", "Инспектируемые части");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActIsolated.Register.InspectedPart");

            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Register.ProvidedDoc", "Предоставленные документы");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActIsolated.Register.ProvidedDoc");

            this.Namespace("GkhGji.DocumentsGji.ActIsolated.Register.ActIsolatedRealObj", "Результаты проверки");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActIsolated.Register.ActIsolatedRealObj");

            #endregion Акты без взаимодействия - реестры

            #endregion Акты без взаимодействия

            #endregion Документы ГЖИ

            #region Деятельность ТСЖ
            this.Namespace("GkhGji.ActivityTsj", "Деятельность ТСЖ");
            this.CRUDandViewPermissions("GkhGji.ActivityTsj");

            this.Namespace("GkhGji.ActivityTsj.Register", "Реестры");
            this.Namespace("GkhGji.ActivityTsj.Register.Statute", "Устав");
            this.Namespace<ActivityTsjStatute>("GkhGji.ActivityTsj.Register.Statute.Field", "Поля");
            this.Permission("GkhGji.ActivityTsj.Register.Statute.Field.StatuteApprovalDate", "Дата утверждения устава");
            this.Permission("GkhGji.ActivityTsj.Register.Statute.Field.StatuteProvisionDate", "Дата предоставления устава");
            this.Permission("GkhGji.ActivityTsj.Register.Statute.Field.StatuteFile", "Файл устава");
            this.Permission("GkhGji.ActivityTsj.Register.Statute.Field.TypeConclusion", "Тип заключения");
            this.Permission("GkhGji.ActivityTsj.Register.Statute.Field.ConclusionNum", "Номер");
            this.Permission("GkhGji.ActivityTsj.Register.Statute.Field.ConclusionDate", "Дата");
            this.Permission("GkhGji.ActivityTsj.Register.Statute.Field.ConclusionFile", "Файл заключения");
            this.Permission("GkhGji.ActivityTsj.Register.Statute.Field.ConclusionDescription", "Описание");

            this.Namespace("GkhGji.ActivityTsj.Register.Member", "Реестр членов ТСЖ/ЖСК");
            this.Namespace<ActivityTsjMember>("GkhGji.ActivityTsj.Register.Member.Field", "Поля");
            this.Permission("GkhGji.ActivityTsj.Register.Member.Field.File", "Файл реестра");
            this.Permission("GkhGji.ActivityTsj.Register.Member.Field.Delete", "Удаление записей");

            #endregion Деятельность ТСЖ

            #region Подготовка к отопительному сезону
            this.Namespace("GkhGji.HeatInputInformation", "Информация о подаче тепла");
            this.CRUDandViewPermissions("GkhGji.HeatInputInformation");

            this.Namespace("GkhGji.WorkWinterCondition", "Подготовка к работе в зимних условиях");
            this.CRUDandViewPermissions("GkhGji.WorkWinterCondition");
            this.Permission("GkhGji.WorkWinterCondition.CopyWorkWinterPeriod_View", "Копирование данных из периода - Просмотр");
            this.Permission("GkhGji.WorkWinterCondition.CopyWorkWinterPeriod_Edit", "Копирование данных из периода - Редактирование");

            this.Namespace("GkhGji.HeatSeason", "Подготовка к отопительному сезону");
            this.CRUDandViewPermissions("GkhGji.HeatSeason");

            this.Namespace("GkhGji.HeatSeason.BoilerRooms", "Котельные");
            this.CRUDandViewPermissions("GkhGji.HeatSeason.BoilerRooms");

            #region Подготовка к отопительному сезону - реестры
            this.Namespace("GkhGji.HeatSeason.Register", "Реестры");

            this.Namespace<HeatSeasonDoc>("GkhGji.HeatSeason.Register.Document", "Документы");
            this.CRUDandViewPermissions("GkhGji.HeatSeason.Register.Document");

            this.Namespace("GkhGji.HeatSeason.Register.Document.Field", "Поля");
            this.Permission("GkhGji.HeatSeason.Register.Document.Field.TypeDocument_Edit", "Тип документа");
            this.Permission("GkhGji.HeatSeason.Register.Document.Field.DocumentNumber_Edit", "Номер документа");
            this.Permission("GkhGji.HeatSeason.Register.Document.Field.DocumentDate_Edit", "Дата документа");
            this.Permission("GkhGji.HeatSeason.Register.Document.Field.File_Edit", "Файл");
            this.Permission("GkhGji.HeatSeason.Register.Document.Field.Description_Edit", "Описание");

            this.Namespace("GkhGji.HeatSeason.Register.Inspection", "Обследование дома");
            this.Permission("GkhGji.HeatSeason.Register.Inspection.View", "Просмотр");
            this.Permission("GkhGji.HeatSeason.Register.Inspection.Create", "Создание записей");

            #endregion Подготовка к отопительному сезону - реестры

            #endregion Подготовку к отопительному сезону

            #region Участники процесса 
            this.Namespace("GkhGji.EDS", "СЭД");

            this.Namespace("GkhGji.EDS.EDSRegistry", "Реестр СЭД");
            this.CRUDandViewPermissions("GkhGji.EDS.EDSRegistry");
            this.Namespace("GkhGji.EDS.EDSRegistrySign", "Реестр документов для подписи");
            this.CRUDandViewPermissions("GkhGji.EDS.EDSRegistrySign");
            #endregion Участники процесса 

            this.Namespace<MKDLicRequest>("Gkh.ManOrgLicense.MKDLicRequest", "Запрос на внесение изменений в реестр лицензий");
            this.Permission("Gkh.ManOrgLicense.MKDLicRequest.View", "Просмотр");
            this.Permission("Gkh.ManOrgLicense.MKDLicRequest.Edit", "Редактирование");
            this.Permission("Gkh.ManOrgLicense.MKDLicRequest.Delete", "Удаление");

            this.Namespace<MKDLicRequest>("Gkh.ManOrgLicense.EDSLicRequest", "Запрос на внесение изменений в реестр лицензий для УК");
            this.CRUDandViewPermissions("Gkh.ManOrgLicense.EDSLicRequest");



            #region Реестр обращений
            this.Namespace("GkhGji.AppealCitizens", "Обращения: Просмотр, создание");
            this.Permission("GkhGji.AppealCitizens.View", "Просмотр");
            this.Permission("GkhGji.AppealCitizens.Create", "Создание записей");

            this.Namespace("GkhGji.AppealCitizens.ShowAppealFilters", "Фильтр по отображению обращений");
            this.Permission("GkhGji.AppealCitizens.ShowAppealFilters.ShowClosedAppeals", "Показать закрытые обращения");

            this.Namespace("GkhGji.AppealCitizens.EmailGji", "Регистрация Email и ПОС");
            this.CRUDandViewPermissions("GkhGji.AppealCitizens.EmailGji");

            this.Namespace("GkhGji.AppealCitizens.AppealCitsInfo", "Логи работы с обращениями");
            this.CRUDandViewPermissions("GkhGji.AppealCitizens.AppealCitsInfo");

            this.Namespace<AppealCits>("GkhGji.AppealCitizensState", "Обращения: Изменение, удаление");
            this.Permission("GkhGji.AppealCitizensState.Edit", "Редактирование");
            this.Permission("GkhGji.AppealCitizensState.Delete", "Удаление записей");

            this.Namespace("GkhGji.AppealCitizensState.Field", "Поля");
            this.Permission("GkhGji.AppealCitizensState.Field.TypeCorrespondent_View", "Тип корреспондента - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.DocumentNumber_View", "Номер обращения - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.DocumentNumber_Edit", "Номер обращения - Редактирование");
            this.Permission("GkhGji.AppealCitizensState.Field.Number_View", "Номер - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.Number_Edit", "Номер - Редактирование");
            this.Permission("GkhGji.AppealCitizensState.Field.Year_View", "Год - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.Year_Edit", "Год - Редактирование");
            this.Permission("GkhGji.AppealCitizensState.Field.DateFrom_Edit", "Дата от - Редактирование");
            this.Permission("GkhGji.AppealCitizensState.Field.PreviousAppealCits_View", "Предыдущее обращение - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.RelatedAppeals_View", "Связанные/аналогичные обращения - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.Department_View", "Отдел - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.Department_Edit", "Отдел - Редактирование");

            this.Namespace<AppealCits>("GkhGji.AppealCitizensState.Answer", "Ответы");
            this.Permission("GkhGji.AppealCitizensState.Answer.Create", "Создание записей");
            this.Permission("GkhGji.AppealCitizensState.Answer.Edit", "Редактирование");
            this.Permission("GkhGji.AppealCitizensState.Answer.Delete", "Удаление записей");
            this.Permission("GkhGji.AppealCitizensState.Answer.Executor_View", "Исполнитель - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Answer.Executor_Edit", "Исполнитель - Редактирование");

            this.Namespace<AppealCits>("GkhGji.AppealCitizensState.Request", "Запросы в компетентные организации");
            this.Permission("GkhGji.AppealCitizensState.Request.Create", "Создание записей");
            this.Permission("GkhGji.AppealCitizensState.Request.Delete", "Удаление записей");

            this.Namespace<AppealCits>("GkhGji.AppealCitizensState.RelatedAppeals", "Связанные/аналогичные обращения");
            this.Permission("GkhGji.AppealCitizensState.RelatedAppeals.View", "Просмотр");

            #endregion

            #region Реестр обращений ФКР
            this.Namespace("GkhGji.AppealCitizensFond", "Обращения ФКР: Просмотр, создание");
            this.Permission("GkhGji.AppealCitizensFond.View", "Просмотр");
            this.Permission("GkhGji.AppealCitizensFond.Create", "Создание записей");
            this.Permission("GkhGji.AppealCitizensFond.CheckBoxShowCloseApp", "Показать закрытые обращения");

            this.Namespace("GkhGji.AppealCitizensFond.AppealCitsInfo", "Логи работы с обращениями ФКР");
            this.CRUDandViewPermissions("GkhGji.AppealCitizensFond.AppealCitsInfo");

            this.Namespace<AppealCits>("GkhGji.AppealCitizensStateFond", "Обращения ФКР: Изменение, удаление");
            this.Permission("GkhGji.AppealCitizensStateFond.Edit", "Редактирование");
            this.Permission("GkhGji.AppealCitizensStateFond.Delete", "Удаление записей");

            this.Namespace("GkhGji.AppealCitizensStateFond.Field", "Поля");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.TypeCorrespondent_View", "Тип корреспондента - Просмотр");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.DocumentNumber_View", "Номер обращения - Просмотр");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.DocumentNumber_Edit", "Номер обращения - Редактирование");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.Number_View", "Номер - Просмотр");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.Number_Edit", "Номер - Редактирование");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.Year_View", "Год - Просмотр");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.Year_Edit", "Год - Редактирование");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.PreviousAppealCits_View", "Предыдущее обращение - Просмотр");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.RelatedAppeals_View", "Связанные/аналогичные обращения - Просмотр");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.Department_View", "Отдел - Просмотр");
            this.Permission("GkhGji.AppealCitizensStateFond.Field.Department_Edit", "Отдел - Редактирование");

            this.Namespace<AppealCits>("GkhGji.AppealCitizensStateFond.Answer", "Ответы");
            this.Permission("GkhGji.AppealCitizensStateFond.Answer.Create", "Создание записей");
            this.Permission("GkhGji.AppealCitizensStateFond.Answer.Edit", "Редактирование");
            this.Permission("GkhGji.AppealCitizensStateFond.Answer.Delete", "Удаление записей");
            this.Permission("GkhGji.AppealCitizensStateFond.Answer.Executor_View", "Исполнитель - Просмотр");
            this.Permission("GkhGji.AppealCitizensStateFond.Answer.Executor_Edit", "Исполнитель - Редактирование");

            this.Namespace<AppealCits>("GkhGji.AppealCitizensStateFond.Request", "Запросы в компетентные организации");
            this.Permission("GkhGji.AppealCitizensStateFond.Request.Create", "Создание записей");
            this.Permission("GkhGji.AppealCitizensStateFond.Request.Delete", "Удаление записей");

            this.Namespace<AppealCits>("GkhGji.AppealCitizensStateFond.RelatedAppeals", "Связанные/аналогичные обращения");
            this.Permission("GkhGji.AppealCitizensStateFond.RelatedAppeals.View", "Просмотр");

            #endregion

            #region Справочники
            this.Namespace("GkhGji.Dict", "Справочники");

            this.Namespace("GkhGji.Dict.TypeOfFeedback", "Типы обратной связи");
            this.CRUDandViewPermissions("GkhGji.Dict.TypeOfFeedback");

            this.Namespace("GkhGji.Dict.ControlList", "Проверочные листы");
            this.CRUDandViewPermissions("GkhGji.Dict.ControlList");

            this.Namespace("GkhGji.Dict.InspectionReason", "Причина вынесения решения");
            this.CRUDandViewPermissions("GkhGji.Dict.InspectionReason");

            this.Namespace("GkhGji.Dict.ControlActivity", "Мероприятия по контролю");
            this.CRUDandViewPermissions("GkhGji.Dict.ControlActivity");

            this.Namespace("GkhGji.Dict.MKDLicTypeRequest", "Типы запросов лицензия МКД");
            this.CRUDandViewPermissions("GkhGji.Dict.MKDLicTypeRequest");

            this.Namespace("GkhGji.Dict.AnswerContent", "Содержание ответа");
            this.CRUDandViewPermissions("GkhGji.Dict.AnswerContent");

            this.Namespace("GkhGji.Dict.ArticleLaw", "Статьи закона ГЖИ");
            this.CRUDandViewPermissions("GkhGji.Dict.ArticleLaw");

            this.Namespace("GkhGji.Dict.ArticleTsj", "Статьи ТСЖ");
            this.CRUDandViewPermissions("GkhGji.Dict.ArticleTsj");

            this.Namespace("GkhGji.Dict.TypeCourt", "Виды суда");
            this.CRUDandViewPermissions("GkhGji.Dict.TypeCourt");

            this.Namespace("GkhGji.Dict.CourtVerdict", "Решения суда");
            this.CRUDandViewPermissions("GkhGji.Dict.CourtVerdict");

            this.Namespace("GkhGji.Dict.ExecutantDoc", "Типы исполнителей");
            this.CRUDandViewPermissions("GkhGji.Dict.ExecutantDoc");

            this.Namespace("GkhGji.Dict.Expert", "Эксперты");
            this.CRUDandViewPermissions("GkhGji.Dict.Expert");

            this.Namespace("GkhGji.Dict.HeatSeasonPeriod", "Периоды отопительного сезона");
            this.CRUDandViewPermissions("GkhGji.Dict.HeatSeasonPeriod");

            this.Namespace("GkhGji.Dict.InspectedPart", "Инспектируемые части");
            this.CRUDandViewPermissions("GkhGji.Dict.InspectedPart");

            this.Namespace("GkhGji.Dict.Instance", "Инстанции");
            this.CRUDandViewPermissions("GkhGji.Dict.Instance");

            this.Namespace("GkhGji.Dict.KindProtocolTsj", "Виды протокола ТСЖ");
            this.CRUDandViewPermissions("GkhGji.Dict.KindProtocolTsj");

            this.Namespace("GkhGji.Dict.KindStatement", "Виды обращений");
            this.CRUDandViewPermissions("GkhGji.Dict.KindStatement");

            this.Namespace("GkhGji.Dict.KindWorkNotif", "Виды работ уведомлений");
            this.CRUDandViewPermissions("GkhGji.Dict.KindWorkNotif");

            this.Namespace("GkhGji.Dict.PlanInsCheck", "Планы инспекционных проверок");
            this.CRUDandViewPermissions("GkhGji.Dict.PlanInsCheck");

            this.Namespace("GkhGji.Dict.PlanJurPerson", "Планы проверок юр.лиц");
            this.CRUDandViewPermissions("GkhGji.Dict.PlanJurPerson");

            this.Namespace("GkhGji.Dict.ProvidedDoc", "Предоставляемые документы");
            this.CRUDandViewPermissions("GkhGji.Dict.ProvidedDoc");

            this.Namespace("GkhGji.Dict.Resolve", "Резолюции");
            this.CRUDandViewPermissions("GkhGji.Dict.Resolve");

            this.Namespace("GkhGji.Dict.RevenueForm", "Формы поступления");
            this.CRUDandViewPermissions("GkhGji.Dict.RevenueForm");

            this.Namespace("GkhGji.Dict.RevenueSource", "Источники поступления");
            this.CRUDandViewPermissions("GkhGji.Dict.RevenueSource");

            this.Namespace("GkhGji.Dict.Sanction", "Санкции");
            this.CRUDandViewPermissions("GkhGji.Dict.Sanction");

            this.Namespace("GkhGji.Dict.StatSubject", "Тематики обращений");
            this.CRUDandViewPermissions("GkhGji.Dict.StatSubject");

            this.Namespace("GkhGji.Dict.StatSubsubject", "Подтематики обращений");
            this.CRUDandViewPermissions("GkhGji.Dict.StatSubsubject");

            this.Namespace("GkhGji.Dict.TypeSurvey", "Типы обследований");
            this.CRUDandViewPermissions("GkhGji.Dict.TypeSurvey");

            this.Namespace("GkhGji.Dict.ViolationGroup", "Группы нарушений");
            this.CRUDandViewPermissions("GkhGji.Dict.ViolationGroup");

            this.Namespace("GkhGji.Dict.ViolationGroup.Violation", "Нарушения");
            this.CRUDandViewPermissions("GkhGji.Dict.ViolationGroup.Violation");

            this.Namespace("GkhGji.Dict.Violation", "Нарушения");
            this.CRUDandViewPermissions("GkhGji.Dict.Violation");
            this.Namespace("GkhGji.Dict.Violation.Field", "Поля");
            this.Permission("GkhGji.Dict.Violation.Field.PpRf170", "ПП РФ №170");
            this.Permission("GkhGji.Dict.Violation.Field.PpRf25", "ПП РФ №25");
            this.Permission("GkhGji.Dict.Violation.Field.PpRf307", "ПП РФ №307");
            this.Permission("GkhGji.Dict.Violation.Field.PpRf491", "ПП РФ №491");
            this.Permission("GkhGji.Dict.Violation.Field.OtherNormativeDocs", "Прочие норм. док.");
            this.Permission("GkhGji.Dict.Violation.Field.GkRf", "ЖК РФ");

            this.Namespace("GkhGji.Dict.FeatureViol", "Характеристики нарушений");
            this.CRUDandViewPermissions("GkhGji.Dict.FeatureViol");

            this.Namespace("GkhGji.Dict.DirectoryERKNM", "Справочники ЕРКНМ");
            this.CRUDandViewPermissions("GkhGji.Dict.DirectoryERKNM");

            this.Namespace("GkhGji.Dict.CompetentOrg", "Компетентные организации");
            this.CRUDandViewPermissions("GkhGji.Dict.CompetentOrg");

            this.Namespace("GkhGji.Dict.RedtapeFlag", "Признак волокиты");
            this.CRUDandViewPermissions("GkhGji.Dict.RedtapeFlag");

            this.Namespace("GkhGji.Dict.GkuTariff", "Тарифы ЖКУ");
            this.CRUDandViewPermissions("GkhGji.Dict.GkuTariff");

            this.Namespace("GkhGji.Dict.KindCheck", "Виды проверок");
            this.CRUDandViewPermissions("GkhGji.Dict.KindCheck");

            this.Namespace("GkhGji.Dict.ActionsRemovViol", "Мероприятия по устранению нарушений");
            this.CRUDandViewPermissions("GkhGji.Dict.ActionsRemovViol");

            this.Namespace("GkhGji.Dict.PlanActionGji", "Планы мероприятий");
            this.CRUDandViewPermissions("GkhGji.Dict.PlanActionGji");

            this.Namespace("GkhGji.Dict.DecisionMakingAuthorityGji", "Органы, принимающие решение по предписанию");
            this.CRUDandViewPermissions("GkhGji.Dict.DecisionMakingAuthorityGji");

            this.Namespace("GkhGji.Dict.SurveyPurpose", "Цели проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.SurveyPurpose");

            this.Namespace("GkhGji.Dict.SurveyObjective", "Задачи проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.SurveyObjective");

            this.Namespace("GkhGji.Dict.AuditPurposeGji", "Цель проведения проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.AuditPurposeGji");

            this.Namespace("GkhGji.Dict.ActivityDirection", "Направления деятельности субъектов проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.ActivityDirection");

            this.Namespace("GkhGji.Dict.DocumentCode", "Коды документов");
            this.CRUDandViewPermissions("GkhGji.Dict.DocumentCode");

            this.Namespace("GkhGji.Dict.SurveySubject", "Предметы проверки");
			this.CRUDandViewPermissions("GkhGji.Dict.SurveySubject");
            this.Permission("GkhGji.Dict.SurveySubject.Columns.Formulation", "Формулировка для плана проверок");

            this.Namespace("GkhGji.Dict.TypeFactViolation", "Виды фактов нарушений");
			this.CRUDandViewPermissions("GkhGji.Dict.TypeFactViolation");

            this.Namespace("GkhGji.Dict.KindBaseDocument", "Виды документов оснований субъектов проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.KindBaseDocument");

            this.Namespace("GkhGji.Dict.SurveySubjectRequirement", "Перечень требований к субъектам проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.SurveySubjectRequirement");

            this.Namespace("GkhGji.Dict.ResolveViolationClaim", "Наименования требований по устранению нарушений");
            this.CRUDandViewPermissions("GkhGji.Dict.ResolveViolationClaim");

            this.Namespace("GkhGji.Dict.NotificationCause", "Причины уведомлений");
            this.CRUDandViewPermissions("GkhGji.Dict.NotificationCause");

            this.Namespace("GkhGji.Dict.MkdManagementMethod", "Способы управления МКД");
            this.CRUDandViewPermissions("GkhGji.Dict.MkdManagementMethod");

            this.Namespace("GkhGji.Dict.OrganMvd", "Органы МВД");
            this.CRUDandViewPermissions("GkhGji.Dict.OrganMvd");

            this.Namespace("GkhGji.Dict.ConcederationResult", "Результат рассмотрения");
            this.CRUDandViewPermissions("GkhGji.Dict.ConcederationResult");

            this.Namespace("GkhGji.Dict.RiskCategory", "Категории риска УК");
            this.CRUDandViewPermissions("GkhGji.Dict.RiskCategory");

            this.Namespace("GkhGji.Dict.PhysicalPersonDocType", "Документы физических лиц (общий)");
            this.CRUDandViewPermissions("GkhGji.Dict.PhysicalPersonDocType");

            #endregion Справочники

            #region Массовая смена статусов документов подготовки к отопительному сезону
            this.Namespace("GkhGji.HeatSeasonDocMassChangeState", "Массовая смена статуса документов подготовки к отопительному сезону");
            this.Permission("GkhGji.HeatSeasonDocMassChangeState.View", "Просмотр");

            #endregion

            #region Отчеты
            this.Namespace("Reports.GJI", "Модуль ГЖИ");
            this.Permission("Reports.GJI.ActPrescription", "Реестр предписаний");
            this.Permission("Reports.GJI.ActPresentation", "Реестр представлений");
            this.Permission("Reports.GJI.ActProtocol", "Реестр протоколов");
            this.Permission("Reports.GJI.ActResolution", "Реестр постановлений");
            this.Permission("Reports.GJI.ControlDocGjiExecution", "Контроль исполнения документов ГЖИ");
            this.Permission("Reports.GJI.FillDocumentsGji", "Отчет \"Заполнение данных в Реестре ГЖИ\"");
            this.Permission("Reports.GJI.Form123", "Отчет \"Форма 123\"");
            this.Permission("Reports.GJI.HeatSeasonReadiness", "Паспорт готовности к отопительному сезону");
            this.Permission("Reports.GJI.MonthlyReportToProsecutors", "Отчет \"Ежемесячный отчет по отделам Инспекции, предоставляемый на сайт Правительства ЯНАО\"");
            this.Permission("Reports.GJI.PrepareHeatSeason", "Акты подготовки к отопительному сезону");
            this.Permission("Reports.GJI.HeatInputInformation", "Информация о подаче тепла");
            this.Permission("Reports.GJI.WorkWinterInfo", "Сведения о подготовке жилищно-коммунального хозяйства к работе в зимних условиях");
            this.Permission("Reports.GJI.ProtocolTotalTable", "Итоговая таблица к отчету по протоколам");
            this.Permission("Reports.GJI.GjiWork", "Работа ГЖИ за период");
            this.Permission("Reports.GJI.ProtocolResponsibility", "Отчет по протоколам");
            this.Permission("Reports.GJI.JournalAppeals", "Отчет журнал обращений");
            this.Permission("Reports.GJI.Form1Control", "Форма 1 Контроль");
            this.Permission("Reports.GJI.ReviewAppealsCits", "Рассмотрение обращений граждан");
            this.Permission("Reports.GJI.SubjectRequests", "Тематика обращений");
            this.Permission("Reports.GJI.StatisticsAppealsCits", "Статистические данные о работе с обращениями ГЖИ");
            this.Permission("Reports.GJI.Form1Control_v2", "Форма № 1-контроль");
            this.Permission("Reports.GJI.MonthlyProsecutorsOfficeReport", "Отчет для прокуратуры (ежемесячный)");
            this.Permission("Reports.GJI.RealObjByMonthlyCr", "Распределение домов по месяцам завершения КР");
            this.Permission("Reports.GJI.ContractObjectCrRegister", "Реестр договоров объекта КР ГЖИ");
            this.Permission("Reports.GJI.ReportOnCourseOfHeatingSeason", "Отчет по ходу отопительного сезона");
            this.Permission("Reports.GJI.SubjectRequestsUkTsj", "Отчет по тематике обращений граждан в разрезе УК, ТСЖ");
            this.Permission("Reports.GJI.CheckingExecutionOfPrescription", "Проверка исполнения предписаний");
            this.Permission("Reports.GJI.RegistryNotificationCommencementBusiness", "Реестр уведомлений о начале предпринимательской деятельности");
            this.Permission("Reports.GJI.Form123Extended", "Форма 123 (расширенная)");
            this.Permission("Reports.GJI.HeatSeasonReceivedDocuments", "Принятые документы по подготовке к отопительному сезону");
            this.Permission("Reports.GJI.InformationOfManagOrg", "Информация об УО (полная)");
            this.Permission("Reports.GJI.AnalyticalReportBySubject", "Аналитический отчёт по тематике");
            this.Permission("Reports.GJI.OlapByInspectionReport", "Отчет по проверкам");
            this.Permission("Reports.GJI.NotificationAttendanceAtProtocolReport", "Уведомление о явке на протокол");
            this.Permission("Reports.GJI.NotificationAttendanceByRepresentativeCheckResultsReport", "Уведомление");
            #endregion

            #region Настройки ГЖИ
            this.Namespace("GkhGji.Settings", "Настройки ГЖИ");
            this.Namespace("GkhGji.Settings.KindCheckRuleReplace", "Правила проставления вида проверки");
            this.Permission("GkhGji.Settings.KindCheckRuleReplace.View", "Просмотр");
            this.Namespace("GkhGji.Settings.DocNumValidationRule", "Правила проставления номера документов ГЖИ");
            this.Permission("GkhGji.Settings.DocNumValidationRule.View", "Просмотр");
            this.Namespace("GkhGji.Settings.Params", "Настройка параметров");
            this.Permission("GkhGji.Settings.Params.View", "Просмотр");

            #endregion

            #region Управление задачами
            this.Namespace("GkhGji.ManagementTask", "Управление задачами");

            this.Namespace("GkhGji.ManagementTask.ReminderInspector", "Правила панели руководителя ГЖИ");
            this.Permission("GkhGji.ManagementTask.ReminderInspector.View", "Просмотр");
            this.Namespace("GkhGji.ManagementTask.ReminderHead", "Правила доски задач Инспектора");
            this.Permission("GkhGji.ManagementTask.ReminderHead.View", "Просмотр");

            #endregion 

            #region Импорты
            this.Namespace("Import.Appeal", "Импорт обращений в жилищную инспекцию");
            this.Permission("Import.Appeal.View", "Просмотр");

            this.Namespace("Import.Tarif", "Импорт тарифов и нормативов");
            this.Permission("Import.Tarif.View", "Просмотр");

            #endregion

            this.Permission("Gkh.ManOrgLicense.Request.FormInsp", "Сформировать проверку");

            this.Namespace("Gkh.Orgs.Contragent.Register.AuditPurpose", "Данные для плана проверок");
            this.Permission("Gkh.Orgs.Contragent.Register.AuditPurpose.View", "Просмотр");
            this.Permission("Gkh.Orgs.Contragent.Register.AuditPurpose.Edit", "Редактировать");
            this.Permission("Gkh.Orgs.Contragent.Register.AuditPurpose.LastInspDate", "Дата прошлой проверки");

            this.Namespace("GkhGji.SurveyPlan", "Реестр планов");
            this.Permission("GkhGji.SurveyPlan.View", "Просмотр");
        }
    }
}