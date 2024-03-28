Ext.define('B4.model.regop.paymentdocument.SberbankPaymentDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'SberbankPaymentDoc'
    },
    fields: [
        { name: 'Id' },
        { name: 'Period' },
        { name: 'AccNumber' },
        { name: 'LastDate' },
        { name: 'Count' },
        { name: 'GUID' },
        { name: 'PaymentDocFile' }
    ]
});