Ext.define('B4.store.regop.paymentdocument.PaymentDocumentsStore', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'DocumentCode' },
        { name: 'PeriodName' },
        { name: 'Link' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'PeriodPaymentDocuments'
    }
});