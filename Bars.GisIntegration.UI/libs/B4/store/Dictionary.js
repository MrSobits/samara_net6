Ext.define('B4.store.Dictionary', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Dictionary'],
    autoLoad: false,
    storeId: 'dictionaryStore',
    model: 'B4.model.Dictionary'
});