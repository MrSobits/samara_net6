Ext.define('B4.model.disposal.DisposalSurveySubject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalSurveySubject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code', defaultValue: null },
        { name: 'Name', defaultValue: null }
    ]
});