Ext.define('B4.model.SyncSession', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SyncLog',
        listAction: 'ListSessions'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'StartTime' },
        { name: 'EndTime' },
        { name: 'TypeIntegration' },
        { name: 'SessionId' }
    ]
});