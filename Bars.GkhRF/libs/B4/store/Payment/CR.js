Ext.define('B4.store.payment.Cr', {
    extend: 'B4.base.Store',
    requires: ['B4.model.payment.Item'],
    autoLoad: false,
    storeId: 'crStore',
    typePayment: 'Cr',
    model: 'B4.model.payment.Item'
});