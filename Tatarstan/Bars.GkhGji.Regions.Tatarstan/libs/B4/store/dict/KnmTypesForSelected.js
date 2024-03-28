Ext.define('B4.store.dict.KnmTypesForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.KnmTypes'],
    autoLoad: false,
    storeId: 'knmTypesForSelectStore',
    model: 'B4.model.dict.KnmTypes'
});