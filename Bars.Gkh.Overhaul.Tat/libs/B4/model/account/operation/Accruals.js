Ext.define('B4.model.account.operation.Accruals', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AccrualsAccountOperation'
    },
    fields: [
         { name: 'Id', useNull: true },
         { name: 'Account' },
         { name: 'AccrualDate' },
         { name: 'TotalDebit' },
         { name: 'TotalCredit' },
         { name: 'OpeningBalance' },
         { name: 'ClosingBalance' }
    ]
});

 



 