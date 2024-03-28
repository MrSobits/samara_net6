Ext.define('B4.store.prescription.ListForStage', {
    extend: 'B4.base.Store',
    fields: ['Id', 'DocumentId', 'TypeDocumentGji', 'DocumentDate', 'DocumentNumber', 'Address'],
    autoLoad: false,
    storeId: 'prescriptionListForStage',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Prescription',
        listAction: 'ListForStage'
    }
});