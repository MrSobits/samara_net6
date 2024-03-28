Ext.define('B4.store.dict.StateForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.StateByType'],
    autoLoad: false,
    storeId: 'stateSelectStore',
    model: 'B4.model.dict.StateByType'
});