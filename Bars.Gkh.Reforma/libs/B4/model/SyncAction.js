Ext.define('B4.model.SyncAction', {
    extend: 'B4.base.Model',
    idProperty: 'Name',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SyncLog',
        listAction: 'ListActions'
    },
    fields: [
        { name: 'SessionId' },
        { name: 'Name' },
        { name: 'Total' },
        { name: 'Failed' },
        { name: 'RequestTime' }
    ]
});