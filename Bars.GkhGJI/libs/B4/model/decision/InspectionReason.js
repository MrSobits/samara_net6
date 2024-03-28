Ext.define('B4.model.decision.InspectionReason', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionInspectionReason'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Decision', defaultValue: null },
        { name: 'InspectionReason', defaultValue: null },
        { name: 'InspectionReasonName' },
        { name: 'Description' }
    ]
});