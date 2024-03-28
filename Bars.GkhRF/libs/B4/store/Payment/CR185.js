Ext.define('B4.store.payment.Cr185', {
    extend: 'B4.base.Store',
    requires: ['B4.model.payment.Item'],
    autoLoad: false,
    storeId: 'cr185Store',
    typePayment: 'Cr185',
    model: 'B4.model.payment.Item'
});