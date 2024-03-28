Ext.define('B4.store.dict.WorkSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Work'],
    autoLoad: false,
    storeId: 'workSelectStore',
    model: 'B4.model.dict.Work'
});