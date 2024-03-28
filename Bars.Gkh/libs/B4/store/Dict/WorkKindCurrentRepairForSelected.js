Ext.define('B4.store.dict.WorkKindCurrentRepairForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.WorkKindCurrentRepair'],
    autoLoad: false,
    storeId: 'workKindCurrentRepairtForSelectedStore',
    model: 'B4.model.dict.WorkKindCurrentRepair'
});