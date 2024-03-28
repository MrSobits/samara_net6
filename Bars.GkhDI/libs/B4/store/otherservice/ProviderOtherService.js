Ext.define('B4.store.otherservice.ProviderOtherService', {
    extend: 'B4.base.Store',
    requires: ['B4.model.otherservice.ProviderOtherService'],
    autoLoad: false,
    storeId: 'providerOtherServiceStore',
    model: 'B4.model.otherservice.ProviderOtherService'
});