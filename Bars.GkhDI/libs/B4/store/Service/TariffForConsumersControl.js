Ext.define('B4.store.service.TariffForConsumersControl', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.TariffForConsumers'],
    autoLoad: false,
    storeId: 'tariffForConsumersControlStore',
    model: 'B4.model.service.TariffForConsumers'
});