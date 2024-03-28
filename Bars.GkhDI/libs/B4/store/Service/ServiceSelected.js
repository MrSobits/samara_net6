Ext.define('B4.store.service.ServiceSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.Base'],
    autoLoad: false,
    storeId: 'serviceSelectedStore',
    model: 'B4.model.service.Base'
});