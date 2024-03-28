Ext.define('B4.store.dict.ControlTypeForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ControlType'],
    autoLoad: false,
    storeId: 'controlTypeSelectStore',
    model: 'B4.model.dict.ControlType'
});