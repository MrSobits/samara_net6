Ext.define('B4.store.dict.RepairProgramObj', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.RepairProgram'],
    autoLoad: false,
    storeId: 'repairProgram',
    model: 'B4.model.dict.RepairProgram',
    proxy: {
        autoLoad: false,
        type: 'ajax',
        url: B4.Url.action('List', 'RepairProgram', {
            forRepairProgramObj: true
        }),
        reader: {
            type: 'json',
            root: 'data',
            idProperty: 'Id',
            totalProperty: 'totalCount',
            successProperty: 'success',
            messageProperty: 'message'
        }
    }
});