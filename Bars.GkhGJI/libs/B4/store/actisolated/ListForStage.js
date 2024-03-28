Ext.define('B4.store.actisolated.ListForStage', {
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
    storeId: 'actIsolatedListForStage',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolated',
        listAction: 'ListForStage'
    }
});