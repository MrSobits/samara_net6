namespace Bars.GkhGji.Regions.Tatarstan.Permissions
{
    using B4;

    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class GkhGjiPermissionMap : PermissionMap
    {
        public GkhGjiPermissionMap()
        {
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.SumAmountWorkRemoval_Edit", "Сумма работ по устранению нарушений");

            this.Namespace("GkhGji.GisCharge", "Интеграция с ГИС ГМП");

            this.Permission("GkhGji.GisCharge.View", "Просмотр - Отправка начисленных штрафов");
            this.Permission("GkhGji.GisCharge.ParamsView", "Просмотр - Настройка параметров");

            this.Namespace("GkhGji.IntegrationErp", "Интеграция с ФГИС ЕРП");
            this.Permission("GkhGji.IntegrationErp.View", "Интеграция с ЕРП");

            this.Namespace("GkhGji.IntegrationErknm", "Интеграция с ФГИС ЕРКНМ");
            this.Permission("GkhGji.IntegrationErknm.View", "Интеграция с ЕРКНМ");

            this.Namespace("GkhGji.IntegrationTor", "Интеграция с ТОР КНД");
            this.Permission("GkhGji.IntegrationTor.View", "Интеграция с ТОР КНД");
            this.Permission("GkhGji.IntegrationTor.SubjectsView", "Отправленные субъекты");
            this.Permission("GkhGji.IntegrationTor.ObjectsView", "Отправленные объекты");

            this.Namespace("GkhGji.Dict.InspectionBaseType", "Основание проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.InspectionBaseType");

            this.Namespace("GkhGji.Dict.WarningBasis", "Основание предостережений");
            this.CRUDandViewPermissions("GkhGji.Dict.WarningBasis");

            this.Namespace("GkhGji.Dict.InspectionBasis", "Основание создания проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.InspectionBasis");

            this.Namespace("GkhGji.Dict.EffectivenessAndPerformanceIndex", "Показатели эффективности и результативности");
            this.CRUDandViewPermissions("GkhGji.Dict.EffectivenessAndPerformanceIndex");

            this.Namespace("GkhGji.Dict.ControlType", "Виды контроля");
            this.CRUDandViewPermissions("GkhGji.Dict.ControlType");

            this.Namespace("GkhGji.Dict.MandatoryReqs", "Обязательные требования");
            this.CRUDandViewPermissions("GkhGji.Dict.MandatoryReqs");

            this.Namespace("GkhGji.Dict.ControlListTypicalAnswer", "Типовые ответы на вопросы проверочного листа");
            this.CRUDandViewPermissions("GkhGji.Dict.ControlListTypicalAnswer");

            this.Namespace("GkhGji.Dict.ControlListTypicalQuestion", "Типовые вопросы проверочного листа");
            this.CRUDandViewPermissions("GkhGji.Dict.ControlListTypicalQuestion");

            this.Namespace("GkhGji.Dict.BudgetClassificationCode", "КБК");
            this.CRUDandViewPermissions("GkhGji.Dict.BudgetClassificationCode");

            this.Namespace("GkhGji.Dict.ConfigurationReferenceInformationKndTor", "Конфигурация справочной информации ТОР КНД");
            this.CRUDandViewPermissions("GkhGji.Dict.ConfigurationReferenceInformationKndTor");

            this.Namespace("GkhGji.Dict.ObjectivesPreventiveMeasures", "Цели профилактических мероприятий");
            this.CRUDandViewPermissions("GkhGji.Dict.ObjectivesPreventiveMeasures");

            this.Namespace("GkhGji.Dict.PreventiveActionItems", "Предметы профилактических мероприятий");
            this.CRUDandViewPermissions("GkhGji.Dict.PreventiveActionItems");

            this.Namespace("GkhGji.Dict.TasksPreventiveMeasures", "Задачи профилактических мероприятий");
            this.CRUDandViewPermissions("GkhGji.Dict.TasksPreventiveMeasures");

            this.Namespace("GkhGji.Dict.KnmTypes", "Виды КНМ");
            this.CRUDandViewPermissions("GkhGji.Dict.KnmTypes");

            this.Namespace("GkhGji.Dict.InspectorPositions", "Должности инспекторов");
            this.CRUDandViewPermissions("GkhGji.Dict.InspectorPositions");

            this.Namespace("GkhGji.Dict.KnmCharacters", "Характеры КНМ");
            this.CRUDandViewPermissions("GkhGji.Dict.KnmCharacters");

            this.Namespace("GkhGji.Dict.RiskCategory", "Категории риска");
            this.CRUDandViewPermissions("GkhGji.Dict.RiskCategory");

            this.Namespace("GkhGji.Dict.ControlObjectType", "Типы объекта контроля");
            this.CRUDandViewPermissions("GkhGji.Dict.ControlObjectType");

            this.Namespace("GkhGji.Dict.ErknmTypeDocument", "Тип документов ЕРКНМ");
            this.CRUDandViewPermissions("GkhGji.Dict.ErknmTypeDocument");

            this.Namespace("GkhGji.Dict.ControlObjectKind", "Виды объекта контроля");
            this.CRUDandViewPermissions("GkhGji.Dict.ControlObjectKind");

            this.Namespace("GkhGji.Dict.KnmAction", "Действия в рамках КНМ");
            this.CRUDandViewPermissions("GkhGji.Dict.KnmAction");

            this.Namespace("GkhGji.EffectivenessAndPerformanceIndexValue", "Значения показателей эффективности и результативности");
            this.Permission("GkhGji.EffectivenessAndPerformanceIndexValue.View", "Просмотр");
            this.Permission("GkhGji.EffectivenessAndPerformanceIndexValue.Delete", "Удаление");
            this.Permission("GkhGji.EffectivenessAndPerformanceIndexValue.Create", "Создание");
            this.Permission("GkhGji.EffectivenessAndPerformanceIndexValue.SendToTor", "Отправка в ТОР");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Action", "Действия");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActCheck.Register.Action");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Action.InspectionAction", "Осмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Action.InspectionAction.Edit", "Редактирование");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Action.SurveyAction", "Опрос");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Action.SurveyAction.Edit", "Редактирование");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Action.InstrExamAction", "Инструментальное обследование");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Action.InstrExamAction.Edit", "Редактирование");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Action.DocRequestAction", "Истребование документов");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Action.DocRequestAction.Edit", "Редактирование");

            this.Namespace("GkhGji.DocumentsGji.ActCheck.Register.Action.ExplanationAction", "Получение письменных объяснений");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Register.Action.ExplanationAction.Edit", "Редактирование");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintState_View", "Статус ознакомления с результатами проверки - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintState_Edit", "Статус ознакомления с результатами проверки - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedDate_View", "Дата ознакомления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedDate_Edit", "Дата ознакомления - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.RefusedToAcquaintPerson_View", "ФИО должностного лица, отказавшегося от ознакомления с актом проверки - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.RefusedToAcquaintPerson_Edit", "ФИО должностного лица, отказавшегося от ознакомления с актом проверки - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedPerson_View", "ФИО должностного лица, ознакомившегося с актом проверки - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedPerson_Edit", "ФИО должностного лица, ознакомившегося с актом проверки - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Protocol.Field.FormatPlace_View", "Место и время составления протокола - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.FormatPlace_Edit", "Место и время составления протокола - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.NotifDeliveredThroughOffice_View", "Вручено через канцелярию - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.NotifDeliveredThroughOffice_Edit", "Вручено через канцелярию - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.FormatDate_View", "Дата вручения (регистрации) уведомления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.FormatDate_Edit", "Дата вручения (регистрации) уведомления - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.NotifNumber_View", "Номер регистрации - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.NotifNumber_Edit", "Номер регистрации - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_View", "Дата и время расмотрения дела - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_Edit", "Дата и время расмотрения дела - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.ProceedingCopyNum_View", "Количество экземпляров - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.ProceedingCopyNum_Edit", "Количество экземпляров - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.ProceedingsPlace_View", "Место рассмотрения дела - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.ProceedingsPlace_Edit", "Место рассмотрения дела - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Remarks_View", "Замечания со стороны нарушителя - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Remarks_Edit", "Замечания со стороны нарушителя - Редактирование");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.Requisites", "Реквизиты");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.Requisites.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.Requisites.Field.PlannedActions_View", "Запланированные действия - просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.Requisites.Field.PlannedActions_Edit", "Запланированные действия - редактирование");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid", "Предметы проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid", "Цели проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid", "Задачи проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid", "НПА проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.Disposal.Field.SendToErp_View", "Отправить в ЕРП - Просмотр");

            #region Предостережение
            this.Namespace<WarningDoc>("GkhGji.DocumentsGji.WarningInspection", "Предостережение");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.WarningInspection");

            this.Namespace("GkhGji.DocumentsGji.WarningInspection.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.BaseWarning_View", "Основание предостережения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.BaseWarning_Edit", "Основание предостережения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.TakingDate_View", "Срок принятия мер о соблюдении требований - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.TakingDate_Edit", "Срок принятия мер о соблюдении требований - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.File_View", "Документ основания - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.File_Edit", "Документ основания - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.ResultText_View", "Результат предостережения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.ResultText_Edit", "Результат предостережения - Редактирование");

            this.Namespace("GkhGji.DocumentsGji.WarningInspection.Field.Official", "Должностные лица");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Official.Autor_View", "Должностное лицо, вынесшее распоряжение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Official.Autor_Edit", "Должностное лицо, вынесшее распоряжение - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Official.Executant_View", "Ответственный за исполнение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Official.Executant_Edit", "Ответственный за исполнение - Редактирование");

            this.Namespace("GkhGji.DocumentsGji.WarningInspection.Field.Output", "Уведомление о направлении предостережения");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutDate_View", "Дата документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutDate_Edit", "Дата документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutNum_View", "Номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutNum_Edit", "Номер документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutDateLatter_View", "Дата исходящего пиьма  - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutDateLatter_Edit", "Дата исходящего пиьма  - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutNumLatter_View", "Номер исходящего письма  - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutNumLatter_Edit", "Номер исходящего письма  - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutSent_View", "Уведомление отправлено - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutSent_Edit", "Уведомление отправлено - Редактирование");

            this.Namespace("GkhGji.DocumentsGji.WarningInspection.Field.Input", "Уведомление об устранении нарушений");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInDate_View", "Дата документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInDate_Edit", "Дата документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInNum_View", "Номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInNum_Edit", "Номер документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInDateLatter_View", "Дата исходящего пиьма  - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInDateLatter_Edit", "Дата исходящего пиьма  - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInNumLatter_View", "Номер исходящего письма  - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInNumLatter_Edit", "Номер исходящего письма  - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInRecived_View", "Уведомление отправлено - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInRecived_Edit", "Уведомление отправлено - Редактирование");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.ObjectionReceived_View", "Получено возражение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Field.Input.ObjectionReceived_Edit", "Получено возражение - Редактирование");

            this.Namespace("GkhGji.DocumentsGji.WarningInspection.Register", "Реестры");
            this.Namespace("GkhGji.DocumentsGji.WarningInspection.Register.WarningBasis", "Основание для предостережения");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Register.WarningBasis.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Register.WarningBasis.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.WarningInspection.Register.Violations", "Нарушение требований");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Register.Violations.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Register.Violations.Delete", "Удаление записей");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Register.Violations.Edit", "Редактирование записей");

            this.Namespace("GkhGji.DocumentsGji.WarningInspection.Register.Annex", "Документы");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Register.Annex.Delete", "Удаление записей");
            this.Permission("GkhGji.DocumentsGji.WarningInspection.Register.Annex.Edit", "Редактирование записей");
            #endregion

            this.Namespace<MotivationConclusion>("GkhGji.DocumentsGji.MotivationConclusion", "Мотивировочное заключение");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.MotivationConclusion");

            this.Namespace("GkhGji.DocumentsGji.MotivationConclusion.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.MotivationConclusion.Field.Autor_View", "Должностное лицо, вынесшее распоряжение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.MotivationConclusion.Field.Autor_Edit", "Должностное лицо, вынесшее распоряжение - Редактирование");
            this.Permission("GkhGji.DocumentsGji.MotivationConclusion.Field.InspectionNumber_View", "Номер распоряжения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.MotivationConclusion.Field.InspectionNumber_Edit", "Номер распоряжения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.MotivationConclusion.Field.Executant_View", "Ответственный за исполнение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.MotivationConclusion.Field.Executant_Edit", "Ответственный за исполнение - Редактирование");
            this.Permission("GkhGji.DocumentsGji.MotivationConclusion.Field.Inspectors_View", "Инспекторы - Просмотр");

            this.Namespace("GkhGji.DocumentsGji.MotivationConclusion.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.MotivationConclusion.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.MotivationConclusion.Annex.Delete", "Удаление записей");

            this.Permission("GkhGji.Inspection.BaseStatement.Field.CheckDate_View", "Дата проверки - Просмотр");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.CheckDate_Edit", "Дата проверки - Редактирование");

            this.Namespace("GkhGji.Inspection.BaseStatement.Register.Contragent", "Органы совместной проверки");
            this.Permission("GkhGji.Inspection.BaseStatement.Register.Contragent.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseStatement.Register.Contragent.Delete", "Удаление записей");

            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.CheckDate_View", "Дата проверки - Просмотр");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.CheckDate_Edit", "Дата проверки - Редактирование");

            this.Namespace("GkhGji.Inspection.BaseProsClaim.Register.Contragent", "Органы совместной проверки");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Register.Contragent.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Register.Contragent.Delete", "Удаление записей");

            this.Permission("GkhGji.Inspection.BaseDispHead.Field.CheckDate_View", "Дата проверки - Просмотр");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.CheckDate_Edit", "Дата проверки - Редактирование");

            this.Namespace("GkhGji.Inspection.BaseDispHead.Register.Contragent", "Органы совместной проверки");
            this.Permission("GkhGji.Inspection.BaseDispHead.Register.Contragent.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseDispHead.Register.Contragent.Delete", "Удаление записей");

            this.Namespace("GkhGji.Inspection", "Проверки");
            this.Namespace<InspectionGji>("GkhGji.Inspection.WarningInspection", "Реестр предостережений");
            this.Permission("GkhGji.Inspection.WarningInspection.ShowCloseInspections", "Показать закрытые проверки");
            this.CRUDandViewPermissions("GkhGji.Inspection.WarningInspection");

            this.Namespace("GkhGji.Inspection.WarningInspection.Registry", "Реестры");

            this.Namespace("GkhGji.Inspection.WarningInspection.Registry.RealityObjects", "Проверяемые дома");
            this.Permission("GkhGji.Inspection.WarningInspection.Registry.RealityObjects.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.WarningInspection.Registry.RealityObjects.View", "Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Registry.RealityObjects.Delete", "Удаление записей");

            this.Namespace("GkhGji.Inspection.WarningInspection.Registry.Contragents", "Органы совместной проверки");
            this.Permission("GkhGji.Inspection.WarningInspection.Registry.Contragents.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.WarningInspection.Registry.Contragents.View", "Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Registry.Contragents.Delete", "Удаление записей");

            this.Namespace("GkhGji.Inspection.WarningInspection.Field", "Поля");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.Date_View", "Дата - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.Date_Edit", "Дата - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.InspectionNumber_View", "Номер - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.InspectionNumber_Edit", "Номер - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.TypeJurPerson_View", "Тип юридического лица - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.TypeJurPerson_Edit", "Тип юридического лица - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.PersonInspection_View", "Объект проверки - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.PersonInspection_Edit", "Объект проверки - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.Contragent_View", "Юридическое лицо - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.Contragent_Edit", "Юридическое лицо - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.PhysicalPerson_View", "Физическое лицо - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.PhysicalPerson_Edit", "Физическое лицо - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.RegistrationNumber_View", "Учетный номер проверки в едином реестре - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.RegistrationNumber_Edit", "Учетный номер проверки в едином реестре - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.RegistrationNumberDate_View", "Дата присвоения учетного номера - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.RegistrationNumberDate_Edit", "Дата присвоения учетного номера - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.CheckDayCount_View", "Срок проверки (количество дней) - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.CheckDayCount_Edit", "Срок проверки (количество дней) - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.CheckDate_View", "Дата проверки - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.CheckDate_Edit", "Дата проверки - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.Inspectors_View", "Инспекторы - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.Inspectors_Edit", "Инспекторы - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.InspectionBasis_View", "Основание - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.InspectionBasis_Edit", "Основание - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.SourceFormType_View", "Форма поступления - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.SourceFormType_Edit", "Форма поступления - Редактирование");

            this.Permission("GkhGji.Inspection.WarningInspection.Field.DocumentName_View", "Наименование документа - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.DocumentName_Edit", "Наименование документа - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.DocumentNumber_View", "Номер документа - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.DocumentNumber_Edit", "Номер документа - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.DocumentDate_View", "Дата документа - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.DocumentDate_Edit", "Дата документа - Редактирование");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.File_View", "Файл - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.File_Edit", "Файл - Редактирование");

            this.Permission("GkhGji.Inspection.WarningInspection.Field.ControlType_View", "Вид контроля - Просмотр");
            this.Permission("GkhGji.Inspection.WarningInspection.Field.ControlType_Edit", "Вид контроля - Редактирование");


            this.Namespace("GkhGji.AppealCitizensState.Field", "Поля");
            this.Permission("GkhGji.AppealCitizensState.Field.IsPrelimentaryCheck_View", "Проведена предварительная проверка - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.IsPrelimentaryCheck_Edit", "Проведена предварительная проверка - Редактирование");

            this.Namespace<TaskDisposal>("GkhGji.DocumentsGji.TaskDisposal", "Задание");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.View", "Просмотр");

            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNumber_Edit", "Номер документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNum_Edit", "Номер - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentYear_View", "Год - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentYear_Edit", "Год - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.LiteralNum_Edit", "Буквенный подномер - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentSubNum_Edit", "Подномер - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.ResponsibleExecution_Edit", "Ответственный за исполнение");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DateStart_Edit", "Период с");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DateEnd_Edit", "Период по");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.OutInspector_View", "Выезд инспектора в командировку - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.OutInspector_Edit", "Выезд инспектора в командировку - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.ObjectVisitStart_View", "Выезд на объект с - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.ObjectVisitStart_Edit", "Выезд на объект с - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.ObjectVisitEnd_View", "Выезд на объект по - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.ObjectVisitEnd_Edit", "Выезд на объект по - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.KindCheck_Edit", "Вид проверки");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.TypeAgreementProsecutor_Edit", "Согласование с прокуратурой");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.TypeAgreementResult_Edit", "Результат согласования");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.IssuedDisposal_Edit", "ДЛ, вынесшее задание");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.Inspectors_Edit", "Инспекторы");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.Notice_Fieldset_View", "Уведомление о проверке - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.Notice_Fieldset_Edit", "Уведомление о проверке - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNumberWithResultAgreement_View", "Номер документа с результатом согласования - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNumberWithResultAgreement_Edit", "Номер документа с результатом согласования - Редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentDateWithResultAgreement_View", "Дата документа с результатом согласования - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Field.DocumentDateWithResultAgreement_Edit", "Дата документа с результатом согласования - Редактирование");

            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register", "Реестры");
            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register.TypeSurvey", "Типы обследования");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.TypeSurvey.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.TypeSurvey.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register.Expert", "Эксперты");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.Expert.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.Expert.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register.ProvidedDoc", "Предоставляемые документы");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.ProvidedDoc.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.ProvidedDoc.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.Annex.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.Annex.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register.Notice", "Уведомление о проверке");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.Notice.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.Notice.Edit", "Редактирование");
            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register.SubjectVerificationGrid", "Предметы проверки");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.SubjectVerificationGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.SubjectVerificationGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.SubjectVerificationGrid.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register.SurveyPurposeGrid", "Цели проверки");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.SurveyPurposeGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.SurveyPurposeGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.SurveyPurposeGrid.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register.SurveyObjectiveGrid", "Задачи проверки");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.SurveyObjectiveGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.SurveyObjectiveGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.SurveyObjectiveGrid.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TaskDisposal.Register.InspFoundationCheckGrid", "НПА проверки");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.InspFoundationCheckGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.InspFoundationCheckGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskDisposal.Register.InspFoundationCheckGrid.Delete", "Удаление записей");

            #region Постановления
            this.Namespace("GkhGji.DocumentsGji.Resolution.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.PatternDict_View", "Шаблон ГИС ГМП - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.PatternDict_Edit", "Шаблон ГИС ГМП - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.BirthDate_View", "Дата рождения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.BirthDate_Edit", "Дата рождения - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.BirthPlace_View", "Место рождения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.BirthPlace_Edit", "Место рождения - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Address_View", "Фактический адрес проживания - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Address_Edit", "Фактический адрес проживания - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.CitizenshipType_View", "Гражданство - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.CitizenshipType_Edit", "Гражданство - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Citizenship_View", "Код страны - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Citizenship_Edit", "Код страны - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.SerialAndNumber_View", "Серия и номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.SerialAndNumber_Edit", "Серия и номер документа - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.IssueDate_View", "Дата выдачи - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.IssueDate_Edit", "Дата выдачи - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.IssuingAuthority_View", "Кем выдан - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.IssuingAuthority_Edit", "Кем выдан - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Company_View", "Место работы, должность - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Company_Edit", "Место работы, должность - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.ChangeReason_View", "Причина изменения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.ChangeReason_Edit", "Причина изменения - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Snils_View", "СНИЛС - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.Snils_Edit", "СНИЛС - Изменение");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.SanctionsDuration_View", "Срок накладываемых санкций - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Resolution.Field.SanctionsDuration_Edit", "Срок накладываемых санкций - Изменение");
            #endregion

            #region Протокол
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.BirthDate_Edit", "Дата рождения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.BirthPlace_Edit", "Место рождения - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.FactAddress_Edit", "Фактический адрес проживания - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.CitizenshipType_Edit", "Гражданство - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Citizenship_Edit", "Код страны - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.SerialAndNumber_Edit", "Серия и номер документа - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.IssueDate_Edit", "Дата выдачи - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.IssuingAuthority_Edit", "Кем выдан - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Company_Edit", "Место работы, должность - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Snils_View", "СНИЛС - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Snils_Edit", "СНИЛС - Изменение");
            #endregion
            
            #region Протокол МВД
            this.Namespace("GkhGji.DocumentsGji.ProtocolMvd.Field", "Поля");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.CitizenshipType_View", "Гражданство - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.CitizenshipType_Edit", "Гражданство - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.Citizenship_View", "Код страны - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.Citizenship_Edit", "Код страны - Редактирование");

            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.Commentary_View", "Комментарий - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ProtocolMvd.Field.Commentary_Edit", "Комментарий - Редактирование");
            #endregion

            #region Протокол ГЖИ
            this.Namespace<TatarstanProtocolGji>("GkhGji.DocumentsGji.TatarstanProtocolGji", "Протоколы ГЖИ РТ");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.TatarstanProtocolGji");
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji.ArticleLaw", "Статьи закона");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.ArticleLaw.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.ArticleLaw.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji.RealityObject", "Адреса правонарушений");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.RealityObject.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Annex.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Annex.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji.Eyewitness", "Сведения о свидетелях и потерпевших");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Eyewitness.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Eyewitness.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Eyewitness.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji.Violation", "Нарушения");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Violation.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Violation.Delete", "Удаление записей");


            this.AddTatarstanProtocolGjiFieldsPermission();
            #endregion

            #region Постановление суда
            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji", "Постановление суда");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Edit", "Изменение записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Annex.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Annex.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji.Eyewitness", "Сведения о свидетелях и потерпевших");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Eyewitness.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Eyewitness.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Eyewitness.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji.Dispute", "Оспаривания");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Dispute.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Dispute.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Dispute.Delete", "Удаление записей");
            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji.Definition", "Определения");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Definition.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Definition.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Definition.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji.PayFine", "Оплаты штрафов");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.PayFine.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.PayFine.Edit", "Редактирование записей");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.PayFine.Delete", "Удаление записей");

            this.AddTatarstanResolutionGjiPermission();

            #endregion

            #region Мотивированное представление КНМ без взаимодействия с контролируемыми лицами
            this.Namespace<MotivatedPresentation>("GkhGji.DocumentsGji.MotivatedPresentationActionIsolated", "Мотивированное представление КНМ без взаимодействия с контролируемыми лицами");
            this.Permission("GkhGji.DocumentsGji.MotivatedPresentationActionIsolated.View", "Просмотр");

            this.Namespace("GkhGji.DocumentsGji.MotivatedPresentationActionIsolated.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.MotivatedPresentationActionIsolated.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.MotivatedPresentationActionIsolated.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.MotivatedPresentationActionIsolated.Register.Annex.Delete", "Удаление записей");
            #endregion

            #region Задание КНМ без взаимодействия с контролируемыми лицами
            this.Namespace<TaskActionIsolated>("GkhGji.DocumentsGji.TaskActionIsolated", "Задание КНМ без взаимодействия с контролируемыми лицами");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.TaskActionIsolated");

            this.Namespace("GkhGji.DocumentsGji.TaskActionIsolated.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.TaskActionIsolated.Register.Requisites", "Реквизиты");

            this.Namespace("GkhGji.DocumentsGji.TaskActionIsolated.Register.Requisites.Fields", "Поля");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Requisites.Fields.AppealCits_Edit", "Обращение гражданина - редактирование");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Requisites.Fields.PlannedAction_View", "Запланированные действия - просмотр");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Requisites.Fields.PlannedAction_Edit", "Запланированные действия - редактирование");

            this.Namespace("GkhGji.DocumentsGji.TaskActionIsolated.Register.Houses", "Дома");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Houses.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Houses.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.TaskActionIsolated.Register.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Annex.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.TaskActionIsolated.Register.Item", "Предмет мероприятия");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Item.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Item.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.TaskActionIsolated.Register.Purposes", "Цели мероприятия");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Purposes.Create", "Создание");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.Purposes.Delete", "Удаление");

            this.Namespace("GkhGji.DocumentsGji.TaskActionIsolated.Register.ArticleLaw", "Статьи закона");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.ArticleLaw.Create", "Создание");
            this.Permission("GkhGji.DocumentsGji.TaskActionIsolated.Register.ArticleLaw.Delete", "Удаление");
            #endregion

            #region Акт КНМ без взаимодействия с контролируемыми лицами
            this.Namespace<ActActionIsolated>("GkhGji.DocumentsGji.ActActionIsolated", "Акт по КНМ без взаимодействия с контролируемыми лицами");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActActionIsolated");
            
            this.Namespace("GkhGji.DocumentsGji.ActActionIsolated.AcquaintInfo", "Ознакомление с результатами мероприятия");
            this.Permission("GkhGji.DocumentsGji.ActActionIsolated.AcquaintInfo.View", "Просмотр");

            this.Namespace("GkhGji.DocumentsGji.ActActionIsolated.Register", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.ActActionIsolated.Register.Defenition", "Определения");
            this.Permission("GkhGji.DocumentsGji.ActActionIsolated.Register.Defenition.Create", "Создание");
            this.Permission("GkhGji.DocumentsGji.ActActionIsolated.Register.Defenition.Delete", "Удаление");
            this.Permission("GkhGji.DocumentsGji.ActActionIsolated.Register.Defenition.View", "Просмотр");
            #endregion

            #region Действия
            this.Namespace("GkhGji.DocumentsGji.ActActionIsolated.Register.Action", "Действия");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActActionIsolated.Register.Action");

            this.Namespace("GkhGji.DocumentsGji.ActActionIsolated.Register.Action.InspectionAction", "Осмотр");
            this.Permission("GkhGji.DocumentsGji.ActActionIsolated.Register.Action.InspectionAction.Edit", "Редактирование");

            this.Namespace("GkhGji.DocumentsGji.ActActionIsolated.Register.Action.InstrExamAction", "Инструментальное обследование");
            this.Permission("GkhGji.DocumentsGji.ActActionIsolated.Register.Action.InstrExamAction.Edit", "Редактирование");

            this.Namespace("GkhGji.DocumentsGji.ActActionIsolated.Register.ActionResult", "Результаты мероприятия");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActActionIsolated.Register.ActionResult");

            this.Namespace("GkhGji.DocumentsGji.ActActionIsolated.Register.Annex", "Приложения");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.ActActionIsolated.Register.Annex");
            #endregion

            #region Проверки по мероприятиям без взаимодействия с контролируемыми лицами
            this.Namespace<InspectionGji>("GkhGji.Inspection.InspectionActionIsolated", "Проверки по мероприятиям без взаимодействия с контролируемыми лицами");
            this.CRUDandViewPermissions("GkhGji.Inspection.InspectionActionIsolated");
            this.Permission("GkhGji.Inspection.InspectionActionIsolated.ShowClosedInspections", "Показать закрытые проверки");

            this.Namespace("GkhGji.Inspection.InspectionActionIsolated.RealityObject", "Проверяемые дома");
            this.Permission("GkhGji.Inspection.InspectionActionIsolated.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.InspectionActionIsolated.RealityObject.Delete", "Удаление записей");

            this.Namespace("GkhGji.Inspection.InspectionActionIsolated.JointInspection", "Органы совместной проверки");
            this.Permission("GkhGji.Inspection.InspectionActionIsolated.JointInspection.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.InspectionActionIsolated.JointInspection.Delete", "Удаление записей");
            #endregion
            
            #region Проверки по профилактическим мероприятиям
            this.Namespace<InspectionGji>("GkhGji.Inspection.InspectionPreventiveAction", "Проверки по профилактическим мероприятиям");
            this.CRUDandViewPermissions("GkhGji.Inspection.InspectionPreventiveAction");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.ShowClosedInspections", "Показать закрытые проверки");

            this.Namespace("GkhGji.Inspection.InspectionPreventiveAction.Fields", "Поля");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.Fields.InspectionNumber_View", "Номер - просмотр");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.Fields.InspectionNumber_Edit", "Номер - редактирование");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.Fields.CheckDate_View", "Дата проверки - просмотр");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.Fields.CheckDate_Edit", "Дата проверки - редактирование");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.Fields.RiskCategory_View", "Категория - просмотр");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.Fields.RiskCategory_Edit", "Категория - редактирование");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.Fields.RiskCategoryStartDate_View", "Дата начала - просмотр");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.Fields.RiskCategoryStartDate_Edit", "Дата начала - редактирование");

            this.Namespace("GkhGji.Inspection.InspectionPreventiveAction.RealityObject", "Проверяемые дома");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.RealityObject.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.RealityObject.Delete", "Удаление записей");

            this.Namespace("GkhGji.Inspection.InspectionPreventiveAction.JointInspection", "Органы совместной проверки");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.JointInspection.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.InspectionPreventiveAction.JointInspection.Delete", "Удаление записей");
            #endregion

            this.Namespace("Gkh.Dictionaries.ProsecutorOffice", "Прокуратуры");
            this.CRUDandViewPermissions("Gkh.Dictionaries.ProsecutorOffice");

            this.Namespace("Gkh.Orgs.ControlOrganization", "Контрольно-надзорные органы");
            this.CRUDandViewPermissions("Gkh.Orgs.ControlOrganization");

            #region Отчеты
            this.Namespace("Reports.GJI", "Модуль ГЖИ");
            this.Permission("Reports.GJI.TatProtocolGjiReport", "Протокол ГЖИ");
            this.Permission("Reports.GJI.CourtResolutionReport", "Постановление суда");
            #endregion

            #region Профилактические мероприятия
            this.Namespace<PreventiveAction>("GkhGji.DocumentsGji.PreventiveActions", "Профилактическое мероприятие");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.PreventiveActions");
            this.Permission("GkhGji.DocumentsGji.PreventiveActions.ShowClosedActions", "Показать закрытые мероприятия");

            this.Namespace<PreventiveActionTask>("GkhGji.DocumentsGji.PreventiveActionTask", "Задание профилактического мероприятия");
            this.Permission("GkhGji.DocumentsGji.PreventiveActionTask.View", "Просмотр");

            this.Namespace<PreventiveActionTask>("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Tasks", "Задачи мероприятия");
            this.Permission("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Tasks.Create", "Создание");
            this.Permission("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Tasks.Delete", "Удаление");

            #region Реестры
            this.Namespace("GkhGji.DocumentsGji.PreventiveActionTask.Registry", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.PreventiveActionTask.Registry.ConsultingQuestion", "Вопросы для консультирования");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.PreventiveActionTask.Registry.ConsultingQuestion");

            this.Namespace<PreventiveActionTask>("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Regulations", "НПА");
            this.Permission("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Regulations.Create", "Создание");
            this.Permission("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Regulations.Delete", "Удаление");

            this.Namespace("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Objectives", "Цели мероприятия");
            this.Permission("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Objectives.Create", "Создание");
            this.Permission("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Objectives.Delete", "Удаление");

            this.Namespace("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Item", "Предмет мероприятия");
            this.Permission("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Item.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.PreventiveActionTask.Registry.Item.Delete", "Удаление записей");
            #endregion
            #endregion

            #region Лист визита
            this.Namespace<VisitSheet>("GkhGji.DocumentsGji.VisitSheet", "Лист визита");
            
            #region Реестры
            this.Namespace("GkhGji.DocumentsGji.VisitSheet.Registry", "Реестры");

            this.Namespace("GkhGji.DocumentsGji.VisitSheet.Registry.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.VisitSheet.Registry.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.VisitSheet.Registry.Annex.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.VisitSheet.Registry.ViolationInfo", "Выявленные нарушения");
            this.CRUDandViewPermissions("GkhGji.DocumentsGji.VisitSheet.Registry.ViolationInfo");
            #endregion
            #endregion

            this.Namespace("GkhGji.AppealCitizensState.ActionIsolated", "КНМ без взаимодействия");
            this.Permission("GkhGji.AppealCitizensState.ActionIsolated.View", "Просмотр");

            this.Namespace("GkhGji.AppealCitizensState.MotivatedPresentation", "Мотивированное представление");
            this.Permission("GkhGji.AppealCitizensState.MotivatedPresentation.View", "Просмотр");
            
            this.Namespace("GkhGji.AppealCitizensState.SoprInformation", "Сведения для СОПР");
            this.Permission("GkhGji.AppealCitizensState.SoprInformation.View", "Просмотр");
            this.Permission("GkhGji.AppealCitizensState.SoprInformation.Delete", "Удаление");
            this.Permission("GkhGji.AppealCitizensState.SoprInformation.CreateAppeal", "Сформировать обращение в СОПР");

            this.Permission("GkhGji.AppealCitizensState.CreateActionIsolated", "Сформировать КНМ без взаимодействия");
            this.Permission("GkhGji.AppealCitizensState.CreateMotivatedPresentation", "Сформировать мотивированное представление");

            this.Permission("GkhGji.AppealCitizens.ShowAppealFilters.View", "Просмотр");
            this.Permission("GkhGji.AppealCitizens.ShowAppealFilters.ShowSoprAppeals", "Показать обращения, имеющие связь с СОПР");
            this.Permission("GkhGji.AppealCitizens.ShowAppealFilters.ShowProcessedAppeals", "Показать обращения, обработанные в СОПР в установленный срок");
            this.Permission("GkhGji.AppealCitizens.ShowAppealFilters.ShowNotProcessedAppeals", "Показать обращения, не обработанные в СОПР в установленный срок");
            this.Permission("GkhGji.AppealCitizens.ShowAppealFilters.ShowInWorkAppeals", "Показать обращения, находящиеся в работе в СОПР");

            this.Permission("GkhGji.AppealCitizens.CreateWarningInspection", "Сформировать предостережение");
            
            this.Namespace("GkhGji.AppealCitizens.WarningInspection", "Предостережения");
            this.Permission("GkhGji.AppealCitizens.WarningInspection.View", "Просмотр");

            #region Мотивированное представление по обращению гражданина
            this.Namespace<MotivatedPresentationAppealCits>("GkhGji.DocumentsGji.MotivatedPresentationAppealCits", "Мотивированное представление по обращению гражданина");
            this.Permission("GkhGji.DocumentsGji.MotivatedPresentationAppealCits.View", "Просмотр");

            this.Namespace("GkhGji.DocumentsGji.MotivatedPresentationAppealCits.Annex", "Приложения");
            this.Permission("GkhGji.DocumentsGji.MotivatedPresentationAppealCits.Annex.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.MotivatedPresentationAppealCits.Annex.Delete", "Удаление записей");
            #endregion
            
            #region Модуль "Обращения граждан"
            this.Namespace("CitizenAppealModule", "Модуль Обращения граждан");
            this.Namespace("CitizenAppealModule.RapidResponseSystem", "Система оперативного реагирования");
            this.Permission("CitizenAppealModule.RapidResponseSystem.ViewRegister", "Доступ к реестру");
            this.Permission("CitizenAppealModule.RapidResponseSystem.View", "Просмотр");
            this.Permission("CitizenAppealModule.RapidResponseSystem.Edit", "Изменение");
            this.Permission("CitizenAppealModule.RapidResponseSystem.ViewAll", "Просмотр всех записей");
            this.Permission("CitizenAppealModule.RapidResponseSystem.Filter", "Блок фильтрации");
            
            this.Namespace<RapidResponseSystemAppealDetails>("CitizenAppealModule.RapidResponseSystem.Fields", "Поля");
            this.Permission("CitizenAppealModule.RapidResponseSystem.Fields.ResponseDate_Edit", "Дата ответа - Изменение");
            this.Permission("CitizenAppealModule.RapidResponseSystem.Fields.Theme_Edit", "Тема - Изменение");
            this.Permission("CitizenAppealModule.RapidResponseSystem.Fields.Response_Edit", "Ответ - Изменение");
            this.Permission("CitizenAppealModule.RapidResponseSystem.Fields.CarriedOutWork_Edit", "Проведенные работы - Изменение");
            this.Permission("CitizenAppealModule.RapidResponseSystem.Fields.ResponseFile_Edit", "Файл - Изменение");

            this.Permission("GkhGji.AppealCitizensState.Field.IsIdentityVerified_View", "Данные личности подтверждены - Просмотр");
            this.Permission("GkhGji.AppealCitizensState.Field.IsIdentityVerified_Edit", "Данные личности подтверждены - Изменение");
            #endregion
        }

        /// <summary>
        /// Ограничения для полей протокола ГЖИ РТ
        /// </summary>
        private void AddTatarstanProtocolGjiFieldsPermission()
        {
            this.Namespace("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields", "Поля");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DocumentNumber_View", "Номер протокола - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DocumentNumber_Edit", "Номер протокола - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DocumentDate_View", "Дата протокола - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DocumentDate_Edit", "Дата протокола - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Municipality_View", "Муниципальное образование - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Municipality_Edit", "Муниципальное образование - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateOffense_View", "Дата правонарушения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateOffense_Edit", "Дата правонарушения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ZonalInspection_View", "Орган ГЖИ, оформивший протокол - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ZonalInspection_Edit", "Орган ГЖИ, оформивший протокол - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TimeOffense_View", "Время правонарушения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TimeOffense_Edit", "Время правонарушения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CheckInspectors_View", "Инспекторы - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CheckInspectors_Edit", "Инспекторы - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateSupply_View", "Дата поступления в ГЖИ - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateSupply_Edit", "Дата поступления в ГЖИ - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Pattern_View", "Шаблон ГИС ГМП - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Pattern_Edit", "Шаблон ГИС ГМП - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.GisUin_View", "УИН - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.AnnulReason_View", "Причина аннулирования - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.AnnulReason_Edit", "Причина аннулирования - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.UpdateReason_View", "Причина изменения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.UpdateReason_Edit", "Причина изменения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Sanction_View", "Вид санкции - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Sanction_Edit", "Вид санкции - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Paided_View", "Штраф оплачен - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Paided_Edit", "Штраф оплачен - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.PenaltyAmount_View", "Сумма штрафа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.PenaltyAmount_Edit", "Сумма штрафа - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateTransferSsp_View", "Дата передачи в ССП - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DateTransferSsp_Edit", "Дата передачи в ССП - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TerminationBasement_View", "Основание прекращения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TerminationBasement_Edit", "Основание прекращения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TerminationDocumentNum_View", "Номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TerminationDocumentNum_Edit", "Номер документа - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Executant_View", "Тип исполнителя - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Executant_Edit", "Тип исполнителя - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Contragent_View", "Контрагент - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Contragent_Edit", "Контрагент - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Ogrn_View", "ОГРН - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Inn_View", "ИНН - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Kpp_View", "КПП - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SettlementAccount_View", "Расчетный счет - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BankName_View", "Банк - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CorrAccount_View", "Корр. счет - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Bik_View", "БИК - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Okpo_View", "ОКПО - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Okonh_View", "ОКОНХ - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Okved_View", "ОКВЭД - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SurName_View", "Фамилия - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SurName_Edit", "Фамилия - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CitizenshipType_View", "Гражданство - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.CitizenshipType_Edit", "Гражданство - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Name_View", "Имя - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Name_Edit", "Имя - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Citizenship_Edit", "Код страны - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Patronymic_View", "Отчество - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Patronymic_Edit", "Отчество - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IdentityDocumentType_View", "Тип документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IdentityDocumentType_Edit", "Тип документа - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SerialAndNumberDocument_View", "Серия и номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.SerialAndNumberDocument_Edit", "Серия и номер документа - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BirthDate_View", "Дата рождения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BirthDate_Edit", "Дата рождения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IssueDate_View", "Дата выдачи - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IssueDate_Edit", "Дата выдачи - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BirthPlace_View", "Место рождения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.BirthPlace_Edit", "Место рождения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IssuingAuthority_View", "Кем выдан - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IssuingAuthority_Edit", "Кем выдан - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Address_View", "Фактический адрес проживания - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Address_Edit", "Фактический адрес проживания - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Company_View", "Место работы, должность, адрес - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Company_Edit", "Место работы, должность, адрес - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.MaritalStatus_View", "Семейное положение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.MaritalStatus_Edit", "Семейное положение - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.RegistrationAddress_View", "Адрес регистрации - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.RegistrationAddress_Edit", "Адрес регистрации - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DependentCount_View", "Количество иждивенцев - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DependentCount_Edit", "Количество иждивенцев - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Salary_View", "Размер зарплаты (пенсии, стипендии) в руб. - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.Salary_Edit", "Размер зарплаты (пенсии, стипендии) в руб. - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ResponsibilityPunishment_View", "Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ResponsibilityPunishment_Edit", "Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateFio_View", "ФИО - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateFio_Edit", "ФИО - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProcurationNumber_View", "Доверенность номер', - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProcurationNumber_Edit", "Доверенность номер', - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProcurationDate_View", "Доверенность дата - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProcurationDate_Edit", "Доверенность дата - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateCompany_View", "Место работы, должность - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateCompany_Edit", "Место работы, должность - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateResponsibilityPunishment_View", "Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.DelegateResponsibilityPunishment_Edit", "Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProtocolExplanation_View", "Объяснение и замечания по содержанию протокола', - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ProtocolExplanation_Edit", "Объяснение и замечания по содержанию протокола', - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.AccusedExplanation_View", "Объяснение лица, в отношении которого возбуждено дело - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.AccusedExplanation_Edit", "Объяснение лица, в отношении которого возбуждено дело - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IsInTribunal_View", "Рассмотрение дела состоится в суде - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.IsInTribunal_Edit", "Рассмотрение дела состоится в суде - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.RejectionSignature_View", "Отказ от подписания протокола - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.RejectionSignature_Edit", "Отказ от подписания протокола - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ResidencePetition_View", "Ходатайство по месту жительства - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.ResidencePetition_Edit", "Ходатайство по месту жительства - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.TribunalName_Edit", "Суд - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanProtocolGji.Fields.OffenseAddress_Edit", "Адрес - Изменение");

        }

        private void AddTatarstanResolutionGjiPermission()
        {
            this.Namespace("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields", "Поля");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentNumber_View", "Номер протокола - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentNumber_Edit", "Номер протокола - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentDate_View", "Дата протокола - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentDate_Edit", "Дата протокола - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentNum_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentNum_Edit", "Номер - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.LiteralNum_View", "Буквенный подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.LiteralNum_Edit", "Буквенный подномер - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentSubNum_View", "Подномер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DocumentSubNum_Edit", "Подномер - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DeliveryDate_View", "Дата вручения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DeliveryDate_Edit", "Дата вручения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.OffenderWas_View", "Нарушитель явился на рассмотрение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.OffenderWas_Edit", "Нарушитель явился на рассмотрение - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TypeInitiativeOrg_View", "Кем вынесено - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TypeInitiativeOrg_Edit", "Кем вынесено - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SectorNumber_View", "Номер участка - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SectorNumber_Edit", "Номер участка - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.FineMunicipality_View", "МО получателя штрафа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.FineMunicipality_Edit", "МО получателя штрафа - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Inspector_View", "Должностное лицо - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Inspector_Edit", "Должностное лицо - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Municipality_View", "Местонахождение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Municipality_Edit", "Местонахождение - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Sanction_View", "Вид санкции - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Sanction_Edit", "Вид санкции - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Paided_View", "Штраф оплачен - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Paided_Edit", "Штраф оплачен - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.PenaltyAmount_View", "Сумма штрафа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.PenaltyAmount_Edit", "Сумма штрафа - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DateTransferSsp_View", "Дата передачи в ССП - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DateTransferSsp_Edit", "Дата передачи в ССП - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TerminationBasement_View", "Основание прекращения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TerminationBasement_Edit", "Основание прекращения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TerminationDocumentNum_View", "Номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.TerminationDocumentNum_Edit", "Номер документа - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Executant_View", "Тип исполнителя - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Executant_Edit", "Тип исполнителя - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Contragent_View", "Контрагент - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Contragent_Edit", "Контрагент - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Ogrn_View", "ОГРН - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Inn_View", "ИНН - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Kpp_View", "КПП - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SettlementAccount_View", "Расчетный счет - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BankName_View", "Банк - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.CorrAccount_View", "Корр. счет - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Bik_View", "БИК - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Okpo_View", "ОКПО - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Okonh_View", "ОКОНХ - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Okved_View", "ОКВЭД - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SurName_View", "Фамилия - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SurName_Edit", "Фамилия - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.CitizenshipType_View", "Гражданство - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.CitizenshipType_Edit", "Гражданство - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Name_View", "Имя - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Name_Edit", "Имя - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Citizenship_Edit", "Код страны - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Patronymic_View", "Отчество - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Patronymic_Edit", "Отчество - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IdentityDocumentType_View", "Тип документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IdentityDocumentType_Edit", "Тип документа - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SerialAndNumberDocument_View", "Серия и номер документа - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.SerialAndNumberDocument_Edit", "Серия и номер документа - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BirthDate_View", "Дата рождения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BirthDate_Edit", "Дата рождения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IssueDate_View", "Дата выдачи - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IssueDate_Edit", "Дата выдачи - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BirthPlace_View", "Место рождения - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.BirthPlace_Edit", "Место рождения - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IssuingAuthority_View", "Кем выдан - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.IssuingAuthority_Edit", "Кем выдан - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Address_View", "Фактический адрес проживания - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Address_Edit", "Фактический адрес проживания - Изменение");

            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Company_View", "Место работы, должность, адрес - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Company_Edit", "Место работы, должность, адрес - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.MaritalStatus_View", "Семейное положение - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.MaritalStatus_Edit", "Семейное положение - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RegistrationAddress_View", "Адрес регистрации - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RegistrationAddress_Edit", "Адрес регистрации - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DependentCount_View", "Количество иждивенцев - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DependentCount_Edit", "Количество иждивенцев - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Salary_View", "Размер зарплаты (пенсии, стипендии) в руб. - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.Salary_Edit", "Размер зарплаты (пенсии, стипендии) в руб. - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ResponsibilityPunishment_View", "Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ResponsibilityPunishment_Edit", "Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateFio_View", "ФИО - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateFio_Edit", "ФИО - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ProcurationNumber_View", "Доверенность номер', - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ProcurationNumber_Edit", "Доверенность номер', - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ProcurationDate_View", "Доверенность дата - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ProcurationDate_Edit", "Доверенность дата - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateCompany_View", "Место работы, должность - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateCompany_Edit", "Место работы, должность - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateResponsibilityPunishment_View", "Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DelegateResponsibilityPunishment_Edit", "Ранее к административной ответственности по ч.1 ст. 20.6 КоАП РФ привлекались - Изменение");

            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ImprovingFact_View", "Смягчающие вину обстоятельства - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.ImprovingFact_Edit", "Смягчающие вину обстоятельства - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DisimprovingFact_View", "Отягчающие вину обстоятельства - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.DisimprovingFact_Edit", "Отягчающие вину обстоятельства - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulinFio_View", "ФИО - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulinFio_Edit", "ФИО - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulingDate_View", "Дата - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulingDate_Edit", "Дата - Изменение");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulingNumber_View", "Номер - Просмотр");
            this.Permission("GkhGji.DocumentsGji.TatarstanResolutionGji.Fields.RulingNumber_Edit", "Номер - Изменение");
        }
    }
}