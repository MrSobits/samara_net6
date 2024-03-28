Ext.define('B4.model.disposal.SurveyObjective', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalSurveyObjective'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Description' },
        { name: 'Name', defaultValue: null },
        { name: 'Code', defaultValue: null }
    ]
});