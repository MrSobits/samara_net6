Ext.define('B4.model.dict.TypeSurveyAdminRegulationGji', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeSurveyAdminRegulationGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'NormativeDoc', defaultValue: null }
    ]
});