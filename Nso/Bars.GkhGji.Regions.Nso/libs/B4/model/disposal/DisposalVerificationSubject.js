Ext.define('B4.model.disposal.DisposalVerificationSubject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalVerificationSubject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name', defaultValue: null },
        { name: 'Code', defaultValue: null },
        { name: 'SurveySubjectId' }
    ]
});