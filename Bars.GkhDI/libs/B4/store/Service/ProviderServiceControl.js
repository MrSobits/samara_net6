Ext.define('B4.store.service.ProviderServiceControl', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.ProviderService'],
    autoLoad: false,
    storeId: 'providerServiceAdditionalStore',
    model: 'B4.model.service.ProviderService'
});