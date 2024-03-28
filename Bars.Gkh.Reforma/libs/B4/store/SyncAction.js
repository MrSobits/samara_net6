Ext.define('B4.store.SyncAction', {
    extend: 'B4.base.Store',
    requires: ['B4.model.SyncAction'],
    autoLoad: false,
    storeId: 'syncActionStore',
    model: 'B4.model.SyncAction'
});