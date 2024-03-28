Ext.define('B4.store.manorg.DispatcherWork', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.WorkMode'],
    autoLoad: false,
    storeId: 'managingDispatcherWorkStore',
    model: 'B4.model.manorg.WorkMode'
});