Ext.define('B4.store.PaymentAgent', {
    extend: 'B4.base.Store',
    requires: ['B4.model.PaymentAgent'],
    autoLoad: false,
    storeId: 'paymentAgentStore',
    model: 'B4.model.PaymentAgent'
});