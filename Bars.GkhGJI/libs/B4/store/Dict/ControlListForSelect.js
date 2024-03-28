Ext.define('B4.store.dict.ControlListForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ControlList'],
    autoLoad: false,
    storeId: 'controlListForSelectStore',
    model: 'B4.model.dict.ControlList'
});