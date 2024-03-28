Ext.define('B4.model.dict.PaymentPenalties', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentPenalties'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Days' },
        { name: 'Percentage' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'DecisionType' }
    ]
});