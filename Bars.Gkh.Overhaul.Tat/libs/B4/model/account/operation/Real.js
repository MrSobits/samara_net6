Ext.define('B4.model.account.operation.Real', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealAccountOperation'
    },
    fields: [
         { name: 'Id', useNull: true },
         { name: 'Account' },
         { name: 'Name' },
         { name: 'OperationDate' },
         { name: 'Sum' },
         { name: 'Receiver' },
         { name: 'Payer' },
         { name: 'Purpose' }
    ]
});