Ext.define('B4.model.appealcits.AppealCitsAdmonition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsAdmonition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'DocumentName', defaultValue: 'Предостережение' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'PerfomanceDate' },
        { name: 'PerfomanceFactDate' },
        { name: 'RealityObject' },
        { name: 'Inspector', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'Executor', defaultValue: null },
        { name: 'AnswerFile', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'Address', defaultValue: null },
        { name: 'AppealNumber' },
        { name: 'DateFrom' },
        { name: 'SentToERKNM', defaultValue: false },
        { name: 'ERKNMID' },
        { name: 'ERKNMGUID' },
        { name: 'KindKND', defaultValue: 10 },
        { name: 'INN' },
        { name: 'KPP' },
        { name: 'PayerType', defaultValue: 30 },
        { name: 'DocumentNumberFiz' },
        { name: 'DocumentSerial' },
        { name: 'PhysicalPersonDocType' },
        { name: 'FIO' },
        { name: 'InspectionReasonERKNM', defaultValue: null },
        { name: 'FizAddress' },
        { name: 'FizINN' },
        { name: 'AccessGuid' },
        { name: 'SignedFile', defaultValue: null },
        { name: 'Signature', defaultValue: null },
        { name: 'Certificate', defaultValue: null },
        { name: 'AnswerFile', defaultValue: null },
        { name: 'SignedAnswerFile', defaultValue: null },
        { name: 'AnswerSignature', defaultValue: null }
    ]
});