Ext.define('B4.model.prescription.Violation', {
    extend: 'B4.model.inspectiongji.ViolStage',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrescriptionViol'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'InspectionViolationId', defaultValue: null },
        { name: 'ViolationGjiPin', defaultValue: null },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'DatePlanRemoval' },
        { name: 'DateFactRemoval' },
        { name: 'SumAmountWorkRemoval' },
        { name: 'Municipality', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectId' },
        { name: 'Action' },
        { name: 'ViolationCount' }, 
        { name: 'ViolationWording' }
    ]
});