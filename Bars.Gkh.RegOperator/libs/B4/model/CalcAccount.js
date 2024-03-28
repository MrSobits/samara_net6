Ext.define('B4.model.CalcAccount', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CalcAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AccountOwner', defaultValue: null },
        { name: 'CreditOrg', defaultValue: null },
        { name: 'DateOpen' },
        { name: 'DateClose', defaultValue: null },
        { name: 'TypeAccount', defaultValue: 10 },
        { name: 'TypeOwner', defaultValue: 10 },
        { name: 'AccountNumber' },
        { name: 'Credit' },
        { name: 'Debt' },
        { name: 'Saldo' },
        { name: 'ChargeTotal' },
        { name: 'PaidTotal' },
        { name: 'PercentSum' },
        { name: 'Address' },
        { name: 'Municipality' },
        { name: 'MoSettlement', defaultValue: null },
        { name: 'RealityObjectId' }
    ]
});