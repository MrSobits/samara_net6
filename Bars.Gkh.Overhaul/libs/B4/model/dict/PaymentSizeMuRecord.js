Ext.define('B4.model.dict.PaymentSizeMuRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentSizeMuRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' }
    ]
});