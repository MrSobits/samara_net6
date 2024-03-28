Ext.define('B4.model.regop.SpecialAccountNonRegop', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'AccountNumber' },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'ChargeTotal' },
        { name: 'PaidTotal' },
        { name: 'Debt' },
        { name: 'Saldo' },
        { name: 'AccountOwner' },
        { name: 'PaymentAccountId' },
        { name: 'RealityObject' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectSpecialOrRegOperatorAccount',
        extraParams: {
            accountType: 'SpecialAccount'
        }
    }
});