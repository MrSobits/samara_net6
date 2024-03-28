Ext.define('B4.model.eds.EDSDocumentRegistry', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'EDSScript',
        listAction: 'ListEDSDocumentsForSign'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'File', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'DocumentGjiId' },
        { name: 'DocumentNumber' },
        { name: 'TypeAnnex', defaultValue: 0 },
        { name: 'SignController' }
    ]
});