Ext.define('B4.model.admincase.Violation', {
    extend: 'B4.model.inspectiongji.ViolStage',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdministrativeCaseViol'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'InspectionViolationId', defaultValue: null },
        { name: 'ViolationGjiPin', defaultValue: null },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'DatePlanRemoval' },
        { name: 'DateFactRemoval' },
        { name: 'Municipality', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectId' },
        { name: 'Description' },
        { name: 'ViolationCount' },
        { name: 'Action' }
    ]
});