Ext.define('B4.model.actionisolated.taskaction.Item', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskActionIsolatedItem'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Task' },
        { name: 'Name' }
    ]
});