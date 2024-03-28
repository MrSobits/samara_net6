Ext.define('B4.store.service.TariffForConsumersHousing', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.TariffForConsumers'],
    autoLoad: false,
    storeId: 'tariffForConsumersHousingStore',
    model: 'B4.model.service.TariffForConsumers'
});