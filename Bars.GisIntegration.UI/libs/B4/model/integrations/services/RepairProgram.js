Ext.define('B4.model.integrations.services.RepairProgram', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Services',
        listAction: 'GetRepairProgramList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Period' },
        { name: 'State' }
    ]
});