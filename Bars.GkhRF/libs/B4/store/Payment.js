Ext.define('B4.store.Payment', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Payment'],
    autoLoad: false,
    storeId: 'paymentStore',
    model: 'B4.model.Payment'
});