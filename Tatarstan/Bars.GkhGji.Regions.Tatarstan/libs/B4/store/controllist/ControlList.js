Ext.define('B4.store.controllist.ControlList', {
    extend: 'B4.base.Store',
    requires: ['B4.model.controllist.ControlList'],
    autoLoad: false,
    storeId: 'controlList',
    model: 'B4.model.controllist.ControlList'
});