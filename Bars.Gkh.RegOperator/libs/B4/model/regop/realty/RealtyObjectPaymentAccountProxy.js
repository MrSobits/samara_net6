Ext.define('B4.model.regop.realty.RealtyObjectPaymentAccountProxy', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'AccountNum' },
        { name: 'BankAccountNum' },
        { name: 'DateClose' },
        { name: 'DateOpen' },
        { name: 'LastOperationDate' },
        { name: 'DebtTotal' },
        { name: 'CreditTotal' },
        { name: 'MoneyLocked' },
        { name: 'CurrentBalance' },
        { name: 'OverdraftLimit' },
        { name: 'Loan' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectPaymentAccount',
        readAction: 'Get'
    }
});
