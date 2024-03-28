namespace Bars.GkhGji.Regions.Tatarstan
{
    using B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("gischarge", "B4.controller.GisCharge", requiredPermission: "GkhGji.GisCharge.View"));
            map.AddRoute(new ClientRoute("gisgmpparams", "B4.controller.GisGmpParams", requiredPermission: "GkhGji.GisCharge.ParamsView"));
            map.AddRoute(new ClientRoute("warninginspection", "B4.controller.WarningInspection", requiredPermission: "GkhGji.Inspection.WarningInspection.View"));
            map.AddRoute(new ClientRoute("gjiinspectionbasis", "B4.controller.dict.InspectionBasis", requiredPermission: "GkhGji.Dict.InspectionBasis.View"));
            map.AddRoute(new ClientRoute("gjiwarningbasis", "B4.controller.dict.WarningBasis", requiredPermission: "GkhGji.Dict.WarningBasis.View"));
            map.AddRoute(new ClientRoute("prosecutoroffice", "B4.controller.dict.ProsecutorOffice", requiredPermission: "Gkh.Dictionaries.ProsecutorOffice.View"));
            map.AddRoute(new ClientRoute("budgetclassificationcode", "B4.controller.dict.BudgetClassificationCode", requiredPermission: "GkhGji.Dict.BudgetClassificationCode.View"));
            map.AddRoute(new ClientRoute("integrationerp", "B4.controller.IntegrationErp", requiredPermission: "GkhGji.IntegrationErp.View"));
            map.AddRoute(new ClientRoute("integrationerknm", "B4.controller.IntegrationErknm", requiredPermission: "GkhGji.IntegrationErknm.View"));
            map.AddRoute(new ClientRoute("effectivenessandperformanceindex", "B4.controller.dict.EffectivenessAndPerformanceIndex", requiredPermission: "GkhGji.Dict.EffectivenessAndPerformanceIndex.View"));
            map.AddRoute(new ClientRoute("effectivenessandperformanceindexvalue", "B4.controller.EffectivenessAndPerformanceIndexValue", requiredPermission: "GkhGji.EffectivenessAndPerformanceIndexValue.View"));
            map.AddRoute(new ClientRoute("controltype", "B4.controller.dict.ControlType", requiredPermission: "GkhGji.Dict.ControlType.View"));
            map.AddRoute(new ClientRoute("controlorganization", "B4.controller.controlorg.ControlOrganization", requiredPermission: "Gkh.Orgs.ControlOrganization.View"));
            map.AddRoute(new ClientRoute("integrationtor", "B4.controller.integrationtor.IntegrationTor", requiredPermission: "GkhGji.IntegrationTor.View"));
            map.AddRoute(new ClientRoute("integrationtorsubjects", "B4.controller.integrationtor.IntegrationTorSubjects", requiredPermission: "GkhGji.IntegrationTor.SubjectsView"));
            map.AddRoute(new ClientRoute("integrationtorobjects", "B4.controller.integrationtor.IntegrationTorObjects", requiredPermission: "GkhGji.IntegrationTor.ObjectsView"));
            map.AddRoute(new ClientRoute("mandatoryreqs", "B4.controller.dict.MandatoryReqs", requiredPermission: "GkhGji.Dict.MandatoryReqs.View"));
            map.AddRoute(new ClientRoute("controllisttypicalanswer", "B4.controller.dict.ControlListTypicalAnswer", requiredPermission: "GkhGji.Dict.ControlListTypicalAnswer.View"));
            map.AddRoute(new ClientRoute("controllisttypicalquestion", "B4.controller.dict.ControlListTypicalQuestion", requiredPermission: "GkhGji.Dict.ControlListTypicalQuestion.View"));
            map.AddRoute(new ClientRoute("configurationreferenceinformationkndtor", "B4.controller.dict.ConfigurationReferenceInformationKndTor", requiredPermission: "GkhGji.Dict.ConfigurationReferenceInformationKndTor.View"));
            map.AddRoute(new ClientRoute("tatarstanprotocolgji", "B4.controller.tatarstanprotocolgji.TatarstanProtocolGji", requiredPermission: "GkhGji.DocumentsGji.TatarstanProtocolGji.View"));
            map.AddRoute(new ClientRoute("tatarstanprotocolgjiedit/{id}", "B4.controller.tatarstanprotocolgji.Navi", requiredPermission: "GkhGji.DocumentsGji.TatarstanProtocolGji.View"));
            map.AddRoute(new ClientRoute("tatarstanprotocolgjiedit/{id}/edit", "B4.controller.tatarstanprotocolgji.Edit", requiredPermission: "GkhGji.DocumentsGji.TatarstanProtocolGji.View"));
            map.AddRoute(new ClientRoute("tatarstanprotocolgjiedit/{id}/resolution", "B4.controller.tatarstanresolutiongji.Edit", requiredPermission: "GkhGji.DocumentsGji.TatarstanProtocolGji.View"));
            map.AddRoute(new ClientRoute("actionisolated", "B4.controller.actionisolated.ActionIsolated", requiredPermission: "GkhGji.DocumentsGji.TaskActionIsolated.View"));
            map.AddRoute(new ClientRoute("preventiveactions", "B4.controller.preventiveaction.PreventiveAction", requiredPermission: "GkhGji.DocumentsGji.PreventiveActions.View"));
            map.AddRoute(new ClientRoute("inspectionactionisolated", "B4.controller.inspectionactionisolated.InspectionActionIsolated", requiredPermission: "GkhGji.Inspection.InspectionActionIsolated.View"));
            map.AddRoute(new ClientRoute("inspectionpreventiveaction", "B4.controller.inspectionpreventiveaction.InspectionPreventiveAction", requiredPermission: "GkhGji.Inspection.InspectionPreventiveAction.View"));
            map.AddRoute(new ClientRoute("objectivespreventivemeasures", "B4.controller.dict.ObjectivesPreventiveMeasure", requiredPermission: "GkhGji.Dict.ObjectivesPreventiveMeasures.View"));
            map.AddRoute(new ClientRoute("preventiveactionitems", "B4.controller.dict.PreventiveActionItems", requiredPermission: "GkhGji.Dict.PreventiveActionItems.View"));
            map.AddRoute(new ClientRoute("taskspreventivemeasures", "B4.controller.dict.TasksPreventiveMeasures", requiredPermission: "GkhGji.Dict.TasksPreventiveMeasures.View"));
            map.AddRoute(new ClientRoute("knmtypes", "B4.controller.dict.KnmTypes", requiredPermission: "GkhGji.Dict.KnmTypes.View"));
            map.AddRoute(new ClientRoute("inspectorpositions", "B4.controller.dict.InspectorPositions", requiredPermission: "GkhGji.Dict.InspectorPositions.View"));
            map.AddRoute(new ClientRoute("knmcharacters", "B4.controller.dict.KnmCharacters", requiredPermission: "GkhGji.Dict.KnmCharacters.View"));
            map.AddRoute(new ClientRoute("riskcategory", "B4.controller.dict.RiskCategory", requiredPermission: "GkhGji.Dict.RiskCategory.View"));
            map.AddRoute(new ClientRoute("controlobjecttype", "B4.controller.dict.ControlObjectType", requiredPermission: "GkhGji.Dict.ControlObjectType.View"));            
            map.AddRoute(new ClientRoute("controlobjectkind", "B4.controller.dict.ControlObjectKind", requiredPermission: "GkhGji.Dict.ControlObjectKind.View"));
            map.AddRoute(new ClientRoute("erknmtypedocument", "B4.controller.dict.ErknmTypeDocument", requiredPermission: "GkhGji.Dict.ErknmTypeDocument.View"));
            map.AddRoute(new ClientRoute("knmaction", "B4.controller.dict.KnmAction", requiredPermission: "GkhGji.Dict.KnmAction.View"));
            map.AddRoute(new ClientRoute("rapidresponsesystemappeal", "B4.controller.rapidresponsesystem.Appeal", requiredPermission: "CitizenAppealModule.RapidResponseSystem.ViewRegister"));
        }
    }
}