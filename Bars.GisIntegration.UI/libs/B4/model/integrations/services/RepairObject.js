Ext.define('B4.model.integrations.services.RepairObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Services',
        listAction: 'GetRepairObjectList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RepairProgramName' },
        { name: 'RepairProgramPeriod' },
        { name: 'RepairProgramState' },
        { name: 'Address' }
    ]
});