Ext.define('B4.model.calcaccount.Credit', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CalcAccountCredit'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Account' },
        { name: 'AccountOwner' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'CreditSum' },
        { name: 'PercentSum' },
        { name: 'PercentRate' },
        { name: 'CreditPeriod' },
        { name: 'CreditDebt' },
        { name: 'PercentDebt' },
        { name: 'Document' }
    ]
});