Ext.define('B4.model.dict.TypeSurveyTaskInspGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyTaskInspGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeSurvey', defaultValue: null },
        { name: 'SurveyObjective' },
        { name: 'Code' }
    ]
});