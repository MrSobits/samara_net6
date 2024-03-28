Ext.define('B4.store.service.TariffForConsumers', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.TariffForConsumers'],
    autoLoad: false,
    storeId: 'tariffForConsumersStore',
    model: 'B4.model.service.TariffForConsumers'
});