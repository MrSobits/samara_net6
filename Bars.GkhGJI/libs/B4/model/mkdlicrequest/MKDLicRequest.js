Ext.define('B4.model.mkdlicrequest.MKDLicRequest', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequest'
    },
    fields: [
        { name: 'Id' },
        { name: 'State'},
        { name: 'ExecutantDocGji'},
        { name: 'Contragent' },
        { name: 'StatementDate', defaultValue: new Date() }, 
        { name: 'StatementNumber' }, 
        { name: 'MKDLicTypeRequest' }, 
        { name: 'Inspector' }, 
        { name: 'Executant' }, 
        { name: 'RealityObject' },
        { name: 'RealityObjects' },
        { name: 'LicStatementResult', defaultValue: 0 },
        { name: 'Description' },
        { name: 'LicStatementResultComment' },
        { name: 'ConclusionNumber' },
        { name: 'Objection', defaultValue: false },
        { name: 'ConclusionDate' },
        { name: 'ObjectionResult', defaultValue: 0 },
        { name: 'RequestFile' },
        { name: 'ConclusionFile' },

        { name: 'PreviousRequest', defaultValue: null },
        { name: 'DocumentNumber' },
        { name: 'QuestionStatus', defaultValue: 0 },
        { name: 'RequestStatus' },
        { name: 'Year', defaultValue: (new Date()).getFullYear() },
        { name: 'ExtensTime' },
        { name: 'CheckTime' },
        { name: 'ControlDateGisGkh' },
        { name: 'NumberGji' },
        { name: 'ZonalInspection', defaultValue: null },

        { name: 'TypeCorrespondent', defaultValue: 50 },
        { name: 'ContragentAddress' },
        { name: 'Email' },
        { name: 'Phone' },
        { name: 'AmountPages' },
        { name: 'KindStatement', defaultValue: null },
        { name: 'QuestionsCount' },
        { name: 'Description' },
        { name: 'PlannedExecDate' },
        { name: 'RedtapeFlag', defaultValue: 0 },
        { name: 'ExecutantTakeDate' },
        { name: 'Comment' },
        { name: 'Executant', defaultValue: null },
        { name: 'ChangeDate' },
        { name: 'SuretyResolve', defaultValue: null },
        { name: 'ApprovalContragent', defaultValue: null },
    ]
});