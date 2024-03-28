Ext.define('B4.store.actisolated.Relations', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.DocumentGjiChildren'],
    storeId: 'actIsolatedRelationsStore',
    model: 'B4.model.DocumentGjiChildren'
});