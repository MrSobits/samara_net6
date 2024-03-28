Ext.define('B4.store.SyncSession', {
    extend: 'B4.base.Store',
    requires: ['B4.model.SyncSession'],
    autoLoad: false,
    storeId: 'syncSessionStore',
    model: 'B4.model.SyncSession',
    sorters: [
    {
        property: 'StartTime',
        direction: 'DESC'
    }]
});