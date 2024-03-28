Ext.define('B4.store.service.TariffForConsumersRepair', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.TariffForConsumers'],
    autoLoad: false,
    storeId: 'tariffForConsumersRepairStore',
    model: 'B4.model.service.TariffForConsumers'
});