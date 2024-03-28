Ext.define('B4.store.actcheck.ListForStage', {
    extend: 'B4.base.Store',
    fields: ['Id', 'DocumentId', 'TypeDocumentGji', 'DocumentDate', 'DocumentNumber', 'Address', 'State'],
    autoLoad: false,
    storeId: 'actcheckListForStage',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheck',
        listAction: 'ListForStage'
    }
});