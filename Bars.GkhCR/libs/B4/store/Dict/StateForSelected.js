Ext.define('B4.store.dict.StateForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StateByType'],
    autoLoad: false,
    storeId: 'stateSelectedStore',
    model: 'B4.model.dict.StateByType'
});