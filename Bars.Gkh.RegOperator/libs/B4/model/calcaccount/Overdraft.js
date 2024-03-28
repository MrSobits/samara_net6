Ext.define('B4.model.calcaccount.Overdraft', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CalcAccountOverdraft'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Account' },
        { name: 'AccountOwner' },
        { name: 'DateStart' },
        { name: 'OverdraftLimit' },
        { name: 'PercentRate' },
        { name: 'OverdraftPeriod' }
    ]
});