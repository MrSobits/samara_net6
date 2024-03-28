Ext.define('B4.store.service.ServiceSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.Base'],
    autoLoad: false,
    storeId: 'serviceSelectStore',
    model: 'B4.model.service.Base'
});