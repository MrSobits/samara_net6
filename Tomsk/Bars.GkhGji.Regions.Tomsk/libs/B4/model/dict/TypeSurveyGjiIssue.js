Ext.define('B4.model.dict.TypeSurveyGjiIssue', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyGjiIssue'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeSurvey', defaultValue: null },
        { name: 'Name' },
        { name: 'Code' }
    ]
});