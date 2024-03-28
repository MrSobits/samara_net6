Ext.define('B4.store.bankstatement.PaymentOrderIn', {
    extend: 'B4.base.Store',
    requires: ['B4.model.bankstatement.PaymentOrderIn'],
    autoLoad: false,
    storeId: 'paymentOrderInStore',
    model: 'B4.model.bankstatement.PaymentOrderIn'
});