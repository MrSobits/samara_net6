Ext.define('B4.store.dict.Municipality', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Municipality'],
    autoLoad: false,
    storeId: 'municipalityStore',
    model: 'B4.model.dict.Municipality'
});