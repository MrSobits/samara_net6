Ext.define('B4.model.protocolgji.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'SendFileToErknm', defaultValue: 10 }
    ]
});