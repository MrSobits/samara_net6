Ext.define('B4.model.dict.TypeSurveyContragentType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyContragentType'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeSurveyGji' },
        { name: 'TypeJurPerson' }
    ]
});