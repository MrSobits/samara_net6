Ext.define('B4.model.longtermprobject.ExecutionLongTermProgram', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ExecutionLongTermProgram',
        listAction: 'List'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Object' },
        { name: 'ProgramName' },
        { name: 'Work' },
        { name: 'Sum' },
        { name: 'Perform' }
    ]
});