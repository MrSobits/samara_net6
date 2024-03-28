Ext.define('B4.model.dict.TypeSurveyInspFoundationCheckGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyInspFoundationCheckGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeSurvey', defaultValue: null },
        { name: 'NormativeDoc' },
        { name: 'Code' }
    ]
});