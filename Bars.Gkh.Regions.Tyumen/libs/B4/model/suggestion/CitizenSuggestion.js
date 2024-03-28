Ext.define('B4.model.suggestion.CitizenSuggestion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CitizenSuggestion'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'State', defaultValue: null },
        { name: 'Number' },
        { name: 'CreationDate' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Rubric', defaultValue: null },
        { name: 'ApplicantFio' },
        { name: 'ApplicantAddress' },
        { name: 'ApplicantPhone' },
        { name: 'ApplicantEmail' },
        { name: 'ProblemPlace' },
        { name: 'Description' },
        { name: 'Address' },
        { name: 'RubricName' },
        { name: 'MunicipalityName' },
        { name: 'AnswerText' },
        { name: 'AnswerDate' },
        { name: 'ExecutorType' },
        { name: 'Executor' },
        { name: 'ExecutorManagingOrganization' },
        { name: 'ExecutorMunicipality' },
        { name: 'ExecutorZonalInspection' },
        { name: 'ExecutorCrFund' },
        { name: 'AnswerFile' },
        { name: 'AllHasAnswer' },
        { name: 'Deadline' },
        { name: 'MessageSubject' },
        { name: 'CategoryPosts' },
        { name: 'RoomAddress'}
    ]
});