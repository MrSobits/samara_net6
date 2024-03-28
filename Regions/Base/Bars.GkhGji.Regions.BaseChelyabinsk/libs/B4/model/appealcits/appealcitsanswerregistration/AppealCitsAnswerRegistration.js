Ext.define('B4.model.appealcits.appealcitsanswerregistration.AppealCitsAnswerRegistration', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsAnswerRegistration',
        listAction: 'GetList'
    },
    fields: [
        { name: 'Id' },
        { name: 'DocumentName' },
        { name: 'AppealState' },
        { name: 'AppealNumber' },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'SignedFile' },
        { name: 'TypeAppealAnswer' },
        { name: 'Correspondent' },
        { name: 'SignerFio' },
        { name: 'Sended' },
        { name: 'Registred' },

        { name: 'Executor', defaultValue: null },
        { name: 'Signer', defaultValue: null },
        { name: 'Addressee', defaultValue: null },
        { name: 'AnswerContent', defaultValue: null },
        { name: 'TypeAppealFinalAnswer', defaultValue: 2 },
        { name: 'SerialNumber' },
        { name: 'Description' },
        { name: 'Description2' },
        { name: 'IsMoved', defaultValue: false },
        { name: 'File', defaultValue: null },
        { name: 'FileDoc', defaultValue: null },
        { name: 'AdditionalInfo', defaultValue: null },
        { name: 'IsUploaded', defaultValue: null },
        { name: 'ExecDate', defaultValue: null },
        { name: 'ExtendDate', defaultValue: null },
        { name: 'ConcederationResult', defaultValue: null },
        { name: 'FactCheckingType', defaultValue: null },
        { name: 'RedirectContragent' },
        { name: 'Address' }
    ]
});