Ext.define('B4.store.actcheck.Relations', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.DocumentGjiChildren'],
    storeId: 'actCheckRelationsStore',
    model: 'B4.model.DocumentGjiChildren'
});