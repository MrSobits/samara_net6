Ext.define('B4.store.otherservice.OtherService', {
    extend: 'B4.base.Store',
    requires: ['B4.model.otherservice.OtherService'],
    autoLoad: false,
    storeId: 'otherServiceStore',
    model: 'B4.model.otherservice.OtherService'
});