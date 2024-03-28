Ext.define('B4.store.belaypolicy.Payment', {
    extend: 'B4.base.Store',
    requires: ['B4.model.belaypolicy.Payment'],
    autoLoad: false,
    storeId: 'belayPolicyPaymentStore',
    model: 'B4.model.belaypolicy.Payment'
});