Ext.define('B4.store.service.Base', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.Base'],
    autoLoad: false,
    groupField: 'TypeGroupServiceDi',
    storeId: 'baseServiceStore',
    model: 'B4.model.service.Base'
});