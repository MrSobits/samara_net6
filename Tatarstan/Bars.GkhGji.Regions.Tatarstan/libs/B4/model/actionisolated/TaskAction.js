Ext.define('B4.model.actionisolated.TaskAction', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskActionIsolated'
    },
    fields: [
        { name: 'InspectionId', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'TypeDocumentGji', defaultValue: 180 },
        { name: 'Stage', defaultValue: 210 },
        { name: 'Municipality' },
        { name: 'Contragent' },
        { name: 'IsPlanDone' },
        { name: 'KindAction' },
        { name: 'TypeBase' },
        { name: 'TypeObject' },
        { name: 'TypeJurPerson' },
        { name: 'PersonName' },
        { name: 'Inn' },
        { name: 'PlanAction' },
        { name: 'AppealCits' },
        { name: 'PlannedActions' },
        { name: 'PlannedActionIds' },
        { name: 'IssuedTask' },
        { name: 'ResponsibleExecution' },
        { name: 'Inspectors' },
        { name: 'InspectorIds' },
        { name: 'ControlType' },
        { name: 'ZonalInspection' },
        { name: 'DateStart' },
        { name: 'TimeStart' },
        { name: 'BaseDocumentName' },
        { name: 'BaseDocumentNumber' },
        { name: 'BaseDocumentDate' },
        { name: 'BaseDocumentFile' }
    ]
});