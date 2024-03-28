Ext.define('B4.model.regop.SuspenseAccount', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'DateReceipt' },
        { name: 'SuspenseAccountTypePayment' },
        { name: 'AccountBeneficiary' },
        { name: 'Sum' },
        { name: 'DetailsOfPayment' },
        { name: 'DistributeState' },
        { name: 'Reason' },
        { name: 'MoneyDirection', defaultValue: 0 }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'SuspenseAccount'
    }
});