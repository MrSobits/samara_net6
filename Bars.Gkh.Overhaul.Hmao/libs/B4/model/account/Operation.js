Ext.define('B4.model.account.Operation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseOperation'
    },
    fields: [
         { name: 'Id', useNull: true },
         { name: 'BankStatement' },
         { name: 'Operation' },
         { name: 'OperationDate' },
         { name: 'Sum' },
         { name: 'Receiver' },
         { name: 'Payer' },
         { name: 'Purpose' },
         { name: 'Number' }
    ]
});