Ext.define('B4.model.appealcits.PrescriptionFond', {
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
        { name: 'DocumentName', defaultValue: 'Предписание ФКР' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'PerfomanceDate' },
        { name: 'Violations' },
        { name: 'Number' },
        { name: 'KindKNDGJI', defaultValue: 0 },
        { name: 'PerfomanceFactDate' },
        { name: 'Inspector', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'SignedFile', defaultValue: null },
        { name: 'Signature', defaultValue: null },
        { name: 'Executor', defaultValue: null },
        { name: 'AnswerFile', defaultValue: null },
        { name: 'SignedAnswerFile', defaultValue: null },
        { name: 'AnswerSignature', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'Address', defaultValue: null },
        { name: 'MassBuildContract' }
    ]
});