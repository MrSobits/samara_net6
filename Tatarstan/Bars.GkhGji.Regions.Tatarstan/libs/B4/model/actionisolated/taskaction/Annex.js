Ext.define('B4.model.actionisolated.taskaction.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskActionIsolatedAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Task' },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'File' }
    ]
});