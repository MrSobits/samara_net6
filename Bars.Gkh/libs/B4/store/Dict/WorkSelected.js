Ext.define('B4.store.dict.WorkSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Work'],
    autoLoad: false,
    storeId: 'workSelectedStore',
    model: 'B4.model.dict.Work'
});