Ext.define('B4.model.surveyplan.Candidate', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SurveyPlanCandidate'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'JuridicalAddress' },
        { name: 'Name' },
        { name: 'Phone' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'PlanMonth' },
        { name: 'PlanYear' },
        { name: 'AuditPurpose' },
        { name: 'Reason' }
    ]
});