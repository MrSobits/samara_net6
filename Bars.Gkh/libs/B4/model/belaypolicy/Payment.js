Ext.define('B4.model.belaypolicy.Payment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BelayPolicyPayment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'PaymentDate' },
        { name: 'DocumentNumber' },
        { name: 'Sum' },
        { name: 'BelayPolicy', defaultValue: null },
        { name: 'FileInfo', defaultValue: null }
    ]
});