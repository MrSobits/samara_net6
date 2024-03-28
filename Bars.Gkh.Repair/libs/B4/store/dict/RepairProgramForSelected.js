Ext.define('B4.store.dict.RepairProgramForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.RepairProgram'],
    autoLoad: false,
    storeId: 'repairProgramForSelected',
    model: 'B4.model.dict.RepairProgram'
});