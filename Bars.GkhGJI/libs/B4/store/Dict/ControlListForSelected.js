Ext.define('B4.store.dict.ControlListForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ControlList'],
    autoLoad: false,
    storeId: 'controlListForSelectedStore',
    model: 'B4.model.dict.ControlList'
});