Ext.define('B4.store.courtpractice.CourtPracticePrescription', {
    extend: 'B4.base.Store',
    requires: ['B4.model.DocumentGji'],
    autoLoad: false,
    storeId: 'courtpracticePrescriptionStore',
    model: 'B4.model.DocumentGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPracticeOperations',
        listAction: 'ListDocs'
    }
});