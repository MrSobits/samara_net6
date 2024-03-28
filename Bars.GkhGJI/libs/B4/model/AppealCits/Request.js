Ext.define('B4.model.appealcits.Request', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsRequest'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'Inspection', defaultValue: null },
        { name: 'CompetentOrg', defaultValue: null },
        { name: 'DocumentName' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'PerfomanceDate' },
        { name: 'PerfomanceFactDate' },
        { name: 'Description' },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'AppealDate' },
        { name: 'SendDate' },
        { name: 'AnswerDate' },
        { name: 'SuretyDate' },
        { name: 'SenderInspector' },
        { name: 'AppealNumber' },
        { name: 'Contragent' },
        { name: 'Signer' }
    ]
});