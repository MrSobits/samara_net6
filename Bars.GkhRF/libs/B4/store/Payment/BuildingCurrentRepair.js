Ext.define('B4.store.payment.BuildingCurrentRepair', {
    extend: 'B4.base.Store',
    requires: ['B4.model.payment.Item'],
    autoLoad: false,
    storeId: 'buildingCurrentRepairStore',
    typePayment: 'BuildingCurrentRepair',
    model: 'B4.model.payment.Item'
});