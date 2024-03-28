Ext.define('B4.store.payment.BuildingRepair', {
    extend: 'B4.base.Store',
    requires: ['B4.model.payment.Item'],
    autoLoad: false,
    storeId: 'buildingRepairStore',
    typePayment: 'BuildingRepair',
    model: 'B4.model.payment.Item'
});