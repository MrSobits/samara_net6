Ext.define('B4.model.actcheck.InspectionAction', {
    extend: 'B4.model.actcheck.Action',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionAction'
    },
    fields: [
        { name: 'ContinueDate' },
        { name: 'ContinueStartTime' },
        { name: 'ContinueEndTime' },
        { name: 'HasViolation' },
        { name: 'HasRemark' }
    ]
});