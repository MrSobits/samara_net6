Ext.define('B4.store.service.WorkRepairList', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.WorkRepairList'],
    autoLoad: false,
    storeId: 'workRepairListStore',
    model: 'B4.model.service.WorkRepairList'
});