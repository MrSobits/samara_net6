Ext.define('B4.model.regop.UnacceptedPayment', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'PersonalAccount' },
        { name: 'Accepted' },
        { name: 'Comment' },
        { name: 'PaymentDate' },
        { name: 'Sum' },
        { name: 'PenaltySum' },
        { name: 'PaymentType' },
        { name: 'DocNumber' },
        { name: 'DocDate' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'UnacceptedPayment'
    }
});