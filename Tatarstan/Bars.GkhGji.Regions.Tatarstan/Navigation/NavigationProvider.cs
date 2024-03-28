namespace Bars.GkhGji.Regions.Tatarstan.Navigation
{
    using Bars.B4;
    using Bars.B4.Navigation;

    public class NavigationProvider : BaseMainMenuProvider
    {
        public override void Init(MenuItem root)
        {
            var integration = root
                .Add("Жилищная инспекция")
                .Add("Интеграция с ГИС ГМП");

            integration
                .Add("Отправка начисленных штрафов", "gischarge")
                .AddRequiredPermission("GkhGji.GisCharge.View");

            integration
                .Add("Настройка параметров", "gisgmpparams")
                .AddRequiredPermission("GkhGji.GisCharge.ParamsView");

            root.Add("Жилищная инспекция")
                .Add("Интеграция с ФГИС ЕРП")
                .Add("Интеграция с ЕРП", "integrationerp")
                .AddRequiredPermission("GkhGji.IntegrationErp.View");

            root.Add("Жилищная инспекция")
                .Add("Интеграция с ФГИС ЕРКНМ")
                .Add("Интеграция с ЕРКНМ", "integrationerknm")
                .AddRequiredPermission("GkhGji.IntegrationErp.View");

            var inspection = root.Add("Жилищная инспекция")
                .Add("Основания проверок");
            
            inspection
                .AddRequiredPermission("GkhGji.Inspection.WarningInspection.View")
                .WithIcon("warninginspection");

            inspection
                .Add("Проверки по мероприятиям без взаимодействия с контролируемыми лицами", "inspectionactionisolated")
                .AddRequiredPermission("GkhGji.Inspection.InspectionActionIsolated.View");

            inspection
                .Add("Проверки по профилактическим мероприятиям", "inspectionpreventiveaction")
                .AddRequiredPermission("GkhGji.Inspection.InspectionPreventiveAction.View");

            root.Add("Жилищная инспекция")
                .Add("Показатели эффективности и результативности")
                .Add("Значения показателей эффективности и результативности", "effectivenessandperformanceindexvalue")
                .AddRequiredPermission("GkhGji.EffectivenessAndPerformanceIndexValue.View");

            var torIntegration = root.Add("Жилищная инспекция")
                .Add("Интеграция с ТОР КНД");

            torIntegration.Add("Интеграция с ТОР КНД", "integrationtor")
                .AddRequiredPermission("GkhGji.IntegrationTor.View");

            torIntegration.Add("Отправленные субъекты", "integrationtorsubjects")
                .AddRequiredPermission("GkhGji.IntegrationTor.SubjectsView");

            torIntegration.Add("Отправленные объекты", "integrationtorobjects")
                .AddRequiredPermission("GkhGji.IntegrationTor.ObjectsView");

            var dicts = root.Add("Справочники").Add("ГЖИ");

            dicts.Add("Основание предостережений", "gjiwarningbasis")
                .AddRequiredPermission("GkhGji.Dict.WarningBasis.View");

            dicts.Add("Основание создания проверки", "gjiinspectionbasis")
                .AddRequiredPermission("GkhGji.Dict.InspectionBasis.View");
            
            dicts.Add("Прокуратуры", "prosecutoroffice")
                .AddRequiredPermission("Gkh.Dictionaries.ProsecutorOffice.View");

            dicts.Add("Показатели эффективности и результативности", "effectivenessandperformanceindex")
                .AddRequiredPermission("GkhGji.Dict.EffectivenessAndPerformanceIndex.View");

            dicts.Add("Виды контроля", "controltype")
                .AddRequiredPermission("GkhGji.Dict.ControlType.View");

            dicts.Add("Типовые ответы на вопросы проверочного листа", "controllisttypicalanswer")
                .AddRequiredPermission("GkhGji.Dict.ControlListTypicalAnswer.View");

            dicts.Add("Типовые вопросы проверочного листа", "controllisttypicalquestion")
                .AddRequiredPermission("GkhGji.Dict.ControlListTypicalQuestion.View");

            dicts.Add("Обязательные требования", "mandatoryreqs")
                .AddRequiredPermission("GkhGji.Dict.MandatoryReqs.View");
                
            dicts.Add("Конфигурация справочной информации ТОР КНД", "configurationreferenceinformationkndtor")
                .AddRequiredPermission("GkhGji.Dict.ConfigurationReferenceInformationKndTor.View");

            dicts.Add("КБК", "budgetclassificationcode")
                .AddRequiredPermission("GkhGji.Dict.BudgetClassificationCode.View");

            dicts.Add("Цели профилактических мероприятий", "objectivespreventivemeasures")
                .AddRequiredPermission("GkhGji.Dict.ObjectivesPreventiveMeasures.View");

            dicts.Add("Предметы профилактических мероприятий", "preventiveactionitems")
                .AddRequiredPermission("GkhGji.Dict.PreventiveActionItems.View");
            
            dicts.Add("Задачи профилактических мероприятий", "taskspreventivemeasures")
                .AddRequiredPermission("GkhGji.Dict.TasksPreventiveMeasures.View");
            
            dicts.Add("Должности инспекторов", "inspectorpositions")
                .AddRequiredPermission("GkhGji.Dict.InspectorPositions.View");
            
            dicts.Add("Виды КНМ", "knmtypes")
                .AddRequiredPermission("GkhGji.Dict.KnmTypes.View");

            dicts.Add("Характеры КНМ", "knmcharacters")
                .AddRequiredPermission("GkhGji.Dict.KnmCharacters.View");

            dicts.Add("Категории риска", "riskcategory")
                .AddRequiredPermission("GkhGji.Dict.RiskCategory.View");
            
            dicts.Add("Типы объекта контроля", "controlobjecttype")
                .AddRequiredPermission("GkhGji.Dict.ControlObjectType.View");
            
            dicts.Add("Виды объекта контроля", "controlobjectkind")
                .AddRequiredPermission("GkhGji.Dict.ControlObjectKind.View");

            dicts.Add("Действия в рамках КНМ", "knmaction")
                .AddRequiredPermission("GkhGji.Dict.KnmAction.View");

            dicts.Add("Тип документов ЕРКНМ", "erknmtypedocument")
                .AddRequiredPermission("GkhGji.Dict.ErknmTypeDocument.View");

            var contragents = root.Add("Участники процесса").Add("Роли контрагента");

            contragents.Add("Контрольно-надзорные органы", "controlorganization")
                .AddRequiredPermission("Gkh.Orgs.ControlOrganization.View");

            var gjiDocuments = root.Add("Жилищная инспекция").Add("Документы");

            gjiDocuments.Add("Протоколы ГЖИ", "tatarstanprotocolgji")
                .AddRequiredPermission("GkhGji.DocumentsGji.TatarstanProtocolGji.View");

            gjiDocuments.Add("КНМ без взаимодействия с контролируемыми лицами", "actionisolated")
                .AddRequiredPermission("GkhGji.DocumentsGji.TaskActionIsolated.View");

            gjiDocuments.Add("Профилактические мероприятия", "preventiveactions")
                .AddRequiredPermission("GkhGji.DocumentsGji.PreventiveActions.View")
                .WithIcon("warninginspection");

            gjiDocuments.Add("Реестр предостережений", "warninginspection")
                .AddRequiredPermission("GkhGji.DocumentsGji.WarningInspection.View");

            root.Add("Обращения")
                .Add("Система оперативного реагирования")
                .Add("Реестр СОПР", "rapidresponsesystemappeal")
                .AddRequiredPermission("CitizenAppealModule.RapidResponseSystem.ViewRegister");
        }
    }
}