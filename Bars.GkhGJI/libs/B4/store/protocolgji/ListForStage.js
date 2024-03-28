Ext.define('B4.store.protocolgji.ListForStage', {
    extend: 'B4.base.Store',
    fields: ['Id', 'DocumentId', 'TypeDocumentGji', 'DocumentDate', 'DocumentNumber', 'Address'],
    autoLoad: false,
    storeId: 'protocolgjiListForStage',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol',
        listAction: 'ListForStage'
    }
});