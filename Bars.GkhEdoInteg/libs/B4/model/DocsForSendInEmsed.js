Ext.define('B4.model.DocsForSendInEmsed', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EdoIntegration',
        listAction: 'ListInspectionDocsAndAnswers'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentName' },
        { name: 'Type' }
    ]
});