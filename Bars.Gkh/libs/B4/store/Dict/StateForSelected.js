Ext.define('B4.store.dict.StateForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.State'],
    autoLoad: false,
    storeId: 'stateForSelectedStore',
    model: 'B4.model.State'
});