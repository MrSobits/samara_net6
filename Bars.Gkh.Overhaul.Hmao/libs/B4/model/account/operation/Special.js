Ext.define('B4.model.account.operation.Special', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialAccountOperation'
    },
    fields: [
         { name: 'Id', useNull: true },
         { name: 'Account' },
         { name: 'Operation' },
         { name: 'OperationDate' },
         { name: 'Sum' },
         { name: 'Receiver' },
         { name: 'Payer' },
         { name: 'Purpose' }
    ]
});