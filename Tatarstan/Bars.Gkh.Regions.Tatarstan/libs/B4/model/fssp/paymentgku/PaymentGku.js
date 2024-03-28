Ext.define('B4.model.fssp.paymentgku.PaymentGku', {
    extend: 'B4.base.Model',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'Litigation',
        listAction: 'PaymentList'  
    },
    
    fields: [
        { name: 'Period'},
        { name: 'AccountNumber'},
        { name: 'DebtSum'},
        { name: 'Accured'},
        { name: 'PayedForPreviousMonth'}
    ]
});