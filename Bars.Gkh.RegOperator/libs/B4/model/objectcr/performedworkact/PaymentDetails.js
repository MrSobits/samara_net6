Ext.define('B4.model.objectcr.performedworkact.PaymentDetails', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id', useNull: true },
        { name: 'IsNew' },
        { name: 'ActPayment' },
        { name: 'SrcFinanceType' },
        { name: 'Balance' },
        { name: 'Payment' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentSrcFinanceDetails'
    }
});