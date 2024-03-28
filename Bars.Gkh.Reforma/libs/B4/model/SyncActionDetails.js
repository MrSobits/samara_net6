Ext.define('B4.model.SyncActionDetails', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SyncLog',
        listAction: 'GetActionDetails'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'RequestTime' },
        { name: 'ResponseTime' },
        { name: 'Success' },
        { name: 'ErrorCode' },
        { name: 'ErrorName' },
        { name: 'ErrorDescription' },
        { name: 'Details' },
        { name: 'FileId' }
    ]
});