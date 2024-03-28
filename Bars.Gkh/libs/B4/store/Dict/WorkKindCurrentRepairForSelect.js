Ext.define('B4.store.dict.WorkKindCurrentRepairForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.WorkKindCurrentRepair'],
    autoLoad: false,
    storeId: 'workKindCurrentRepairtForSelectStore',
    model: 'B4.model.dict.WorkKindCurrentRepair'
});