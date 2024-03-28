Ext.define('B4.model.regop.BankStatementAccountNumber', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'AccountNumber' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'BankAccountStatement',
        listAction: 'ListAccountNumbers'
    }
});