Ext.define('B4.store.service.WorkToSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.WorkTo'],
    autoLoad: false,
    storeId: 'workToSelectedStore',
    model: 'B4.model.dict.WorkTo'
});