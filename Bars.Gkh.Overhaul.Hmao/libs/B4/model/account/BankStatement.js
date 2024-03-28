Ext.define('B4.model.account.BankStatement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AccBankStatement'
    },
    fields: [
         { name: 'Id', useNull: true },
         { name: 'BankAccount' },
         { name: 'Number' },
         { name: 'DocumentDate' },
         { name: 'BalanceOut' },
         { name: 'BalanceIncome' },
         { name: 'LastOperationDate' },
         { name: 'State', defaultValue: null }
    ]
});