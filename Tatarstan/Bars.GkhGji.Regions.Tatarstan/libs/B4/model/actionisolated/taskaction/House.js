Ext.define('B4.model.actionisolated.taskaction.House', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskActionIsolatedRealityObject',
        actionName: 'List'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'Area' }
    ]
});