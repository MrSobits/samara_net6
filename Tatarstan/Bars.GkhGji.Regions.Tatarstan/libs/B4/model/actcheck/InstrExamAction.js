Ext.define('B4.model.actcheck.InstrExamAction', {
    extend: 'B4.model.actcheck.Action',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InstrExamAction'
    },
    fields: [
        { name: 'Territory' },
        { name: 'Premise' },
        { name: 'TerritoryAccessDenied' },
        { name: 'HasViolation' },
        { name: 'UsingEquipment' },
        { name: 'HasRemark' }
    ]
});