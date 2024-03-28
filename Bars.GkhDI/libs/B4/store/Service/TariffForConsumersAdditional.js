Ext.define('B4.store.service.TariffForConsumersAdditional', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.TariffForConsumers'],
    autoLoad: false,
    storeId: 'tariffForConsumersAdditionalStore',
    model: 'B4.model.service.TariffForConsumers'
});