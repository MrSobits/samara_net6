Ext.define('B4.model.regop.Transfer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Transfer'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Amount' },
        { name: 'PaymentDate' },
        { name: 'OperationDate' },
        { name: 'Reason' },
        { name: 'OriginatorName' }
    ]
});