Ext.define('B4.store.bankstatement.PaymentOrder', {
    extend: 'B4.base.Store',
    requires: ['B4.model.bankstatement.PaymentOrder'],
    autoLoad: false,
    storeId: 'paymentOrderStore',
    model: 'B4.model.bankstatement.PaymentOrder'
});