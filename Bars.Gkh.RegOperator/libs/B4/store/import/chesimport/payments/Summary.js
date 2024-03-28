Ext.define('B4.store.import.chesimport.payments.Summary', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChesImport',
        listAction: 'ListPaymentInfo'
    },
    fields: [
        { name: 'Name' },
        { name: 'WalletType' },
        { name: 'Paid' },
        { name: 'Cancelled' },
        { name: 'Refund' },
        { name: 'Sum' },
        { name: 'Count' },
    ]
});