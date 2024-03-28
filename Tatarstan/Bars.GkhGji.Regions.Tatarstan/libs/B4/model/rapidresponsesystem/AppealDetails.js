Ext.define('B4.model.rapidresponsesystem.AppealDetails', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RapidResponseSystemAppealDetails'
    },
    fields: [
        { name: 'Id'},
        { name: 'State'},
        { name: 'Number'},
        { name: 'Municipality'},
        { name: 'Address'},
        { name: 'RealityObjectId'},
        { name: 'AppealCitsRealityObject' },
        { name: 'ContragentId'},
        { name: 'ContragentName'},
        { name: 'ReceiptDate'},
        { name: 'ControlPeriod'},
        { name: 'AppealDate'},
        { name: 'Subjects'},
        { name: 'TypeCorrespondent'},
        { name: 'Correspondent'},
        { name: 'CorrespondentAddress'},
        { name: 'CorrespondentEmail'},
        { name: 'CorrespondentFlatNum'},
        { name: 'CorrespondentPhone'},
        { name: 'AppealKind'},
        { name: 'QuestionsCount'},
        { name: 'ProblemDescription'},
        { name: 'AppealFileName'},
        { name: 'AppealFileId'},
        { name: 'AppealDetailsId'},
        { name: 'RapidResponseSystemAppeal' },
        { name: 'IsWarningControlPeriod' }
    ]
});