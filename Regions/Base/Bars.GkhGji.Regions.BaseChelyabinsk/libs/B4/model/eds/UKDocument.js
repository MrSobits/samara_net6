Ext.define('B4.model.eds.UKDocument', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'UKDocument'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EDSInspection', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'MessageCheck', defaultValue: 0 },
        { name: 'DocumentSend' },
        { name: 'DocumentDelivered' }
    ]
});