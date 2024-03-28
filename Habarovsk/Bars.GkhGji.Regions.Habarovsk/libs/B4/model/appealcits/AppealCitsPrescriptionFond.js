Ext.define('B4.model.appealcits.AppealCitsPrescriptionFond', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsPrescriptionFond'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'DocumentName'},
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'PerfomanceDate' },
        { name: 'PerfomanceFactDate' },
        { name: 'Inspector', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'Signature', defaultValue: null },
        { name: 'Executor', defaultValue: null },
        { name: 'AnswerFile', defaultValue: null },
        { name: 'SignedAnswerFile', defaultValue: null },
        { name: 'AnswerSignature', defaultValue: null },
        { name: 'MassBuildContract' }
    ]
});