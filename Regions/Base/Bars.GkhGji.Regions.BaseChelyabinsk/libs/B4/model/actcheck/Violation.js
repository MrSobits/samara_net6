Ext.define('B4.model.actcheck.Violation', {
    extend: 'B4.model.inspectiongji.ViolStage',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckViolation'
    },
    fields: [
        { name: 'ActObject', defaultValue: null },
        { name: 'InspectionViolationId', defaultValue: null },
        { name: 'ViolationGjiPin', defaultValue: null },
        { name: 'ViolationGjiName' },
        { name: 'ViolationGjiId' },
        { name: 'DatePlanRemoval' },
        { name: 'DateFactRemoval' },
        { name: 'Municipality', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'ActionsRemovViolName' },
        { name: 'Features' },
        { name: 'CodesPin' },
        { name: 'ViolationDescription' }
    ]
});