Ext.define('B4.model.protocol197.Violation', {
    extend: 'B4.model.inspectiongji.ViolStage',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol197Violation'
    },
    fields: [
        { name: 'InspectionViolationId', defaultValue: null },
        { name: 'ViolationGjiPin', defaultValue: null },
        { name: 'ViolationGjiName' },
        { name: 'ViolationGjiId' },
        { name: 'DatePlanRemoval' },
        { name: 'DateFactRemoval' },
        { name: 'Features' },
        { name: 'CodesPin' },
        { name: 'Description' },
        { name: 'ViolationDescription' },
        { name: 'Protocol197' }
    ]
});