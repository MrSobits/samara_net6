Ext.define('B4.model.inspectiongji.ViolStage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionGjiViolStage'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentGji', defaultValue: null },
        { name: 'InspectionViolation', defaultValue: null },
        { name: 'TypeViolationStage', defaultValue: 10 },
        { name: 'InspectionViolationId', defaultValue: null },
        { name: 'ViolationGjiPin', defaultValue: null },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'DatePlanRemoval' },
        { name: 'DateFactRemoval' },
        { name: 'Municipality', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectId' },
        { name: 'ERPGuid' },
        { name: 'Description' },
        { name: 'ViolationCount' },
        { name: 'Action' }
    ]
});