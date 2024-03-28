Ext.define('B4.store.service.ProviderServiceHousing', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.ProviderService'],
    autoLoad: false,
    storeId: 'providerServiceCommunalStore',
    model: 'B4.model.service.ProviderService'
});