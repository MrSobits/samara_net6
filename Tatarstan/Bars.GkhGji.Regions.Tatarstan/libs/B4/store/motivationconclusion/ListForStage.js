Ext.define('B4.store.motivationconclusion.ListForStage', {
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
    storeId: 'motivationconclusionListForStageStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MotivationConclusion',
        listAction: 'ListForStage'
    }
});