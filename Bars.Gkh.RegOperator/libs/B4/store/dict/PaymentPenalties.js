Ext.define('B4.store.dict.PaymentPenalties', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.PaymentPenalties'],
    autoLoad: false,
    storeId: 'paymentPenalStore',
    model: 'B4.model.dict.PaymentPenalties'
});