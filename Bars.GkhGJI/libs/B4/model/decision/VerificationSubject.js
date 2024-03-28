Ext.define('B4.model.decision.VerificationSubject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionVerificationSubject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Decision', defaultValue: null },
        { name: 'Name', defaultValue: null },
        { name: 'Code', defaultValue: null },
        { name: 'SurveySubject' }
    ]
});