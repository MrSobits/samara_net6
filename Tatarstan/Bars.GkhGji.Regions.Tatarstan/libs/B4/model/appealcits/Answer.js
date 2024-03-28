Ext.define('B4.model.appealcits.Answer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsAnswer'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'Executor', defaultValue: null },
        { name: 'Addressee', defaultValue: null },
        { name: 'AnswerContent', defaultValue: null },
        { name: 'DocumentName' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'State', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'IsSent', defaultValue: 30 }
    ]
});