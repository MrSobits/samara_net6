Ext.define('B4.model.actsurvey.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActSurveyAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActSurvey', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'MessageCheck', defaultValue: 0 },
        { name: 'DocumentSend' },
        { name: 'DocumentDelivered' }

    ]
});