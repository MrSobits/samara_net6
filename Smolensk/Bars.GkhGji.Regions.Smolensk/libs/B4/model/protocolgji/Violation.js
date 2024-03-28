Ext.define('B4.model.protocolgji.Violation', {
    extend: 'B4.model.inspectiongji.ViolStage',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolViolation'
    },
    fields: [
        { name: 'InspectionViolationId', defaultValue: null },
        { name: 'ViolationGjiPin', defaultValue: null },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'DatePlanRemoval' },
        { name: 'DateFactRemoval' },
        { name: 'Municipality', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectId' },
        { name: 'ViolationCount' },
        { name: 'Description' },
        { name: 'ViolationWording' }
    ]
});