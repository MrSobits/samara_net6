Ext.define('B4.model.surveyplan.Contragent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SurveyPlanContragent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'JuridicalAddress' },
        { name: 'Name' },
        { name: 'Phone' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Ogrn' },
        { name: 'PlanMonth' },
        { name: 'PlanYear' },
        { name: 'AuditPurpose' },
        { name: 'Reason' },
        { name: 'ObjectEditDate' },
        { name: 'IsExcluded' },
        { name: 'ExclusionReason' }
    ]
});