Ext.define('B4.store.import.chesimport.PaymentSummary', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChesImport',
        listAction: 'ListPaymentInfo'
    },
    fields: [
        { name: 'WalletType' },
        { name: 'Paid' },
        { name: 'Cancelled' },
        { name: 'Refund' },
        { name: 'Sum' }
    ]
});