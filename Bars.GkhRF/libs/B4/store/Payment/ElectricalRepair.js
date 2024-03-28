Ext.define('B4.store.payment.ElectricalRepair', {
    extend: 'B4.base.Store',
    requires: ['B4.model.payment.Item'],
    autoLoad: false,
    storeId: 'electricalRepairStore',
    typePayment: 'ElectricalRepair',
    model: 'B4.model.payment.Item'
});