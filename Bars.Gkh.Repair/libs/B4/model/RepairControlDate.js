Ext.define('B4.model.RepairControlDate', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RepairControlDate'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProgramCr' },
        { name: 'WorkName' },
        { name: 'Work' },
        { name: 'ProgramName'},
        { name: 'Date' }
    ]
});