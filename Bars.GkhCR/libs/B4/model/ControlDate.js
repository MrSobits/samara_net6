Ext.define('B4.model.ControlDate', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlDate'
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