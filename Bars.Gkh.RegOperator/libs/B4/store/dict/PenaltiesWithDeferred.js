Ext.define('B4.store.dict.PenaltiesWithDeferred', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.PenaltiesWithDeferred'],
    autoLoad: false,
    storeId: 'penaltiesDeferredStore',
    model: 'B4.model.dict.PenaltiesWithDeferred'
});