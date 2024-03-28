Ext.define('B4.store.DeletedObjectCr', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ObjectCr'],
    autoLoad: false,
    storeId: 'deletedObjectCrStore',
    model: 'B4.model.ObjectCr'
});