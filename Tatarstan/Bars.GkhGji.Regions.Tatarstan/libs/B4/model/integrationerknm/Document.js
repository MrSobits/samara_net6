Ext.define('B4.model.integrationerknm.Document', {
    extend: 'B4.model.baseintegration.Document',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ErknmIntegration',
        listAction: 'DocumentList'
    },
    fields: [
        { name: 'ErknmGuid' },
        { name: 'ErknmRegistrationDate' }
    ]
});