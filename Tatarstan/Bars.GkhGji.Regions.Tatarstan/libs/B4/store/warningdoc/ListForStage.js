Ext.define('B4.store.warningdoc.ListForStage', {
    extend: 'B4.base.Store',
    fields: [
        'Id',
        'DocumentId',
        'TypeDocumentGji',
        'DocumentDate',
        'DocumentNumber',
        'Address',
        'State'
    ],
    autoLoad: false,
    storeId: 'warningdocListForStageStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WarningDoc',
        listAction: 'ListForStage'
    }
});