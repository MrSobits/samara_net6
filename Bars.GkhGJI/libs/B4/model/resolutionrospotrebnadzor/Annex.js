Ext.define('B4.model.resolutionrospotrebnadzor.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionRospotrebnadzorAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution', useNull: false },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'DocumentName' },
        { name: 'Description' },
        { name: 'MessageCheck', defaultValue: 0 },
        { name: 'DocumentSend' },
        { name: 'DocumentDelivered' }
    ]
});