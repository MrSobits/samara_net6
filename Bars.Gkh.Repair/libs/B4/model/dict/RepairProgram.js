Ext.define('B4.model.dict.RepairProgram', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RepairProgram'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name'}, 
        { name: 'Period' },
        { name: 'PeriodName' },
        { name: 'TypeVisibilityProgramRepair', defaultValue: 10 },
        { name: 'TypeProgramRepairState', defaultValue: 10 }
    ]
});