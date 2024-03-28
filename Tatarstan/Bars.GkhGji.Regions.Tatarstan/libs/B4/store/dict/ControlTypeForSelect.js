Ext.define('B4.store.dict.ControlTypeForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ControlType'],
    autoLoad: false,
    storeId: 'controlTypeForSelectStore',
    model: 'B4.model.dict.ControlType'
});