Ext.define('B4.model.objectcr.performedworkact.Payment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PerformedWorkActPayment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Document' },
        { name: 'PerformedWorkAct' },
        { name: 'DateDisposal' },
        { name: 'DatePayment' },
        { name: 'Sum', defaultValue: 0 },
        { name: 'Paid', defaultValue: 0 },
        { name: 'Percent' },
        { name: 'HandMade', defaultValue: true },
        { name: 'TypeActPayment', defaultValue: 10 }
    ]
});