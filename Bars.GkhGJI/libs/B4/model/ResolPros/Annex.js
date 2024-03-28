Ext.define('B4.model.resolpros.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolProsAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ResolPros', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'MessageCheck', defaultValue: 0 },
        { name: 'DocumentSend' },
        { name: 'DocumentDelivered' }
    ]
});