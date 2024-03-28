Ext.define('B4.store.courtpractice.DocsForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.DocumentGji'],
    autoLoad: false,
    storeId: 'courtpracticeDocumentGjiStore',
    model: 'B4.model.DocumentGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPracticeOperations',
        listAction: 'ListDocsForSelect'
    }
});