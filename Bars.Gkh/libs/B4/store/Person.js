Ext.define('B4.store.Person', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Person'],
    autoLoad: false,
    storeId: 'persontStore',
    model: 'B4.model.Person'
});