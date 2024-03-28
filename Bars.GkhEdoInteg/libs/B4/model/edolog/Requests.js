Ext.define('B4.model.edolog.Requests', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'LogRequests'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'ObjectCreateDate' },
        { name: 'ObjectEditDate' },
        { name: 'TimeExecution', defaultValue: null },
        { name: 'Uri' },
        { name: 'Count' },
        { name: 'CountAdded' },
        { name: 'CountUpdated' },
        { name: 'File', defaultValue: null }
    ]
});