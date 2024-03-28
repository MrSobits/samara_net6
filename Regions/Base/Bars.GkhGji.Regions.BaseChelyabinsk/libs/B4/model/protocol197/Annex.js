Ext.define('B4.model.protocol197.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol197Annex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol197', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'TypeAnnex' },
        { name: 'MessageCheck', defaultValue: 0 },
        { name: 'DocumentSend' },
        { name: 'DocumentDelivered' }
      
    ]
});