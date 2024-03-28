Ext.define('B4.store.claimwork.pretension.DebtPayment', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'PaymentDate' },
        { name: 'PaymentType' },
        { name: 'Sum' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PretensionClw',
        listAction: 'GetDebtPersAccPayments'
    }
});