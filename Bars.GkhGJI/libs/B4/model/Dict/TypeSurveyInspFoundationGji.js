Ext.define('B4.model.dict.TypeSurveyInspFoundationGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyInspFoundationGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeSurvey', defaultValue: null },
        { name: 'NormativeDoc' },
        { name: 'Code' }
    ]
});