Ext.define('B4.model.dict.TypeSurveyGoalInspGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyGoalInspGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeSurvey', defaultValue: null },
        { name: 'SurveyPurpose' },
        { name: 'Code' }
    ]
});