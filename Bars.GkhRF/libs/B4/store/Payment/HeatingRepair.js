Ext.define('B4.store.payment.HeatingRepair', {
    extend: 'B4.base.Store',
    requires: ['B4.model.payment.Item'],
    autoLoad: false,
    storeId: 'heatingRepairStore',
    typePayment: 'HeatingRepair',
    model: 'B4.model.payment.Item'
});