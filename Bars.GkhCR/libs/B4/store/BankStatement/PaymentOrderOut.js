Ext.define('B4.store.bankstatement.PaymentOrderOut', {
    extend: 'B4.base.Store',
    requires: ['B4.model.bankstatement.PaymentOrderOut'],
    autoLoad: false,
    storeId: 'paymentOrderOutStore',
    model: 'B4.model.bankstatement.PaymentOrderOut'
});