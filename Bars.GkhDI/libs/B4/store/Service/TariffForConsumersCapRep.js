Ext.define('B4.store.service.TariffForConsumersCapRep', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.TariffForConsumers'],
    autoLoad: false,
    storeId: 'tariffForConsumersCapRepStore',
    model: 'B4.model.service.TariffForConsumers'
});